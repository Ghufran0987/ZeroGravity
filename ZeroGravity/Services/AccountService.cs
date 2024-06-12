using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZeroGravity.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Helpers;
using ZeroGravity.Infrastructure;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.Accounts;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using AccountResponse = ZeroGravity.Models.Accounts.AccountResponse;
using BC = BCrypt.Net.BCrypt;
using RefreshToken = ZeroGravity.Db.Models.Users.RefreshToken;

namespace ZeroGravity.Services
{
    public class AccountService : IAccountService
    {
        private readonly ZeroGravityContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<AccountService> _stringLocalizer;
        private readonly ILogger _logger;
        private string _origin;
        private readonly List<string> _launchSettingsAppUrls = new List<string>();

        public AccountService(
            ZeroGravityContext context,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IEmailService emailService,
            IStringLocalizer<AccountService> stringLocalizer,
            ILogger<AccountService> logger,
            IRepository<ZeroGravityContext> repository,
            IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _stringLocalizer = stringLocalizer;
            _logger = logger;

            var commonConfig = config.GetSection("Common").Get<CommonConfig>();
            _origin = commonConfig.Hostname;
            if (!string.IsNullOrEmpty(_origin))
            {
                _logger.LogInformation($"Origin is {_origin}");
            }

#if DEBUG
            var c = config.GetSection("Common").Get<CommonConfig>();
            var host = c.Hostname;

            //_launchSettingsAppUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").ToList();
            _launchSettingsAppUrls.Add(host);
#endif
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            string tempPass = BC.HashPassword(model.Password);

            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if(account == null)
            {
                throw new AccountNotFoundException(_stringLocalizer[MessageType.AccountNotFound]);
            }

            if(!account.IsVerified)
            {
                throw new AccountNotVerifiedException(_stringLocalizer[MessageType.AccountNotVerified]);
            }

            if(!BC.Verify(model.Password, account.PasswordHash))
            {
                throw new EmailOrPasswordIncorrectException(_stringLocalizer[MessageType.MailOrPasswordIncorrect]);
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(account);
            var refreshToken = GenerateRefreshToken(ipAddress);

            CleanUpRefreshTokens(account);

            // save refresh token
            account.RefreshTokens.Add(refreshToken);
            _context.Update(account);
            _context.SaveChanges();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, account) = GetRefreshToken(token);

            // replace old refresh token with a new one and save
            var now = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = now;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            CleanUpRefreshTokens(account);

            account.RefreshTokens.Add(newRefreshToken);

            _context.Update(account);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = GenerateJwtToken(account);

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            response.RefreshTokenExpiration = newRefreshToken.Expires;
            return response;
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = GetRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(account);
            _context.SaveChanges();
        }

        public void Register(RegisterRequest model, string origin)
        {
            // validate
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {
                // send already registered error in email to prevent account enumeration
                SendAlreadyRegisteredEmail(model.Email, origin);
                throw new AppException(_stringLocalizer[MessageType.MailAlreadyRegistered, model.Email]);
            }

            if (model.Password.Length < AccountAndPasswordConstants.PasswordMinimumLength)
            {
                throw new AppException(_stringLocalizer[MessageType.PasswordTooShort, AccountAndPasswordConstants.PasswordMinimumLength]);
            }

            // Check that the email address is associated with a verified subscription purchase
            ConfirmedSensorPurchaseUserData confirmedSensorPurchaseUserData = _context.ConfirmedSensorPurchaseUsers.Where(_ => _.Email == model.Email).FirstOrDefault();
            if (confirmedSensorPurchaseUserData == null || confirmedSensorPurchaseUserData.VerifiedDateTime == null)
            {
                throw new AppException(_stringLocalizer[MessageType.VerificationFailed, model.Email]);
            }

            // map model to new account object
            var account = _mapper.Map<Account>(model);

            account.Role = Role.User;
            account.Created = DateTime.UtcNow;
            account.CompletedFirstUseWizard = false;
            account.VerificationToken = RandomTokenString();

            // Now that we are pre-verifying the email mark it as verified
            account.Verified = DateTime.UtcNow;

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            // save account
            _context.Accounts.Add(account);
            _context.SaveChanges();

            // send email
            // no longer needed as we are pre-verifying the email.
            //SendVerificationEmail(account, origin);
        }

        public void VerifyEmail(string token, bool isEmailChangeRequest = false)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token);

            if (account == null) throw new AppException(_stringLocalizer[MessageType.VerificationFailed]);

            account.Verified = DateTime.UtcNow;

            if (!isEmailChangeRequest)
            {
                account.VerificationToken = null;
            }

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            // create reset token that expires after 1 day
            account.ResetToken = RandomTokenString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(24);
            _context.Accounts.Update(account);
            _context.SaveChanges();

            // send email
            SendPasswordResetEmail(account, origin);
        }

        public void ValidateResetToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null)
                throw new AppException(_stringLocalizer[MessageType.InvalidToken]);
        }

        public void ResetPassword(string token, string email, string newPassword)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == token &&
                x.ResetTokenExpires > DateTime.UtcNow &&
                x.Email == email);

            if (account == null)
                throw new AppException(_stringLocalizer[MessageType.InvalidToken]);

            // update password and remove reset token
            account.PasswordHash = BC.HashPassword(newPassword);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public bool ConfirmPassword(PasswordConfirmRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Id == model.UserId);

            if (account == null)
            {
                throw new AppException(_stringLocalizer[MessageType.AccountNotFound]);
            }

            if (!BC.Verify(model.Password, account.PasswordHash))
            {
                // wrong pass
                return false;
            }

            return true;
        }

        public IEnumerable<AccountResponse> GetAll()
        {
            var accounts = _context.Accounts;
            return _mapper.Map<IList<AccountResponse>>(accounts);
        }

        public AccountResponse GetById(int id)
        {
            var account = GetAccount(id);
            return _mapper.Map<AccountResponse>(account);
        }

        public AccountResponse Create(CreateRequest model)
        {
            // validate
            if (_context.Accounts.Any(x => x.Email == model.Email))
                throw new AppException(_stringLocalizer[MessageType.MailAlreadyRegistered, model.Email]);

            // map model to new account object
            var account = _mapper.Map<Account>(model);
            account.Created = DateTime.UtcNow;
            account.Verified = DateTime.UtcNow;

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            // save account
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return _mapper.Map<AccountResponse>(account);
        }

        public AccountResponse Update(int id, UpdateRequest model)
        {
            var account = GetAccount(id);

            // validate
            if (account.Email != model.Email && _context.Accounts.Any(x => x.Email == model.Email))
                throw new AppException(_stringLocalizer[MessageType.MailAlreadyTaken, model.Email]);

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                account.PasswordHash = BC.HashPassword(model.Password);

            // copy model to account and save
            _mapper.Map(model, account);
            account.Updated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return _mapper.Map<AccountResponse>(account);
        }

        public bool UpdateWizardState(int id, UpdateWizardRequest model)
        {
            var account = GetAccount(id);

            account.CompletedFirstUseWizard = model.CompletedFirstUseWizard;
            account.OnBoardingDate = DateTime.UtcNow;

            _context.Accounts.Update(account);

            var changes = _context.SaveChanges();

            return changes != 0;
        }

        public void Delete(int id)
        {
            var account = GetAccount(id);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public void NewEmailRequest(int id, UpdateEmailRequest model, string origin)
        {
            var account = GetAccount(id);

            // validate
            if (_context.Accounts.Any(x => x.Email == model.NewEmail))
                throw new AppException(_stringLocalizer[MessageType.MailAlreadyTaken, model.NewEmail]);

            account.VerificationToken = RandomTokenString();

            account.NewEmail = model.NewEmail;
            account.Updated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();

            SendVerificationEmail(account, origin, true);
        }

        public void UpdateEmail(string token)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token);

            if (account == null) throw new AppException(_stringLocalizer[MessageType.VerificationFailed]);

            account.Verified = DateTime.UtcNow;
            account.VerificationToken = null;
            account.Email = account.NewEmail;
            account.NewEmail = null;

            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void UpdatePassword(int id, PasswordChangeRequest model)
        {
            var account = GetAccount(id);

            if (account == null)
                throw new AppException(_stringLocalizer[MessageType.AccountNotFound]);

            if (!BC.Verify(model.OldPassword, account.PasswordHash) || string.IsNullOrEmpty(model.OldPassword))
                throw new AppException(_stringLocalizer[MessageType.PasswordOldWrong]);

            if (string.IsNullOrEmpty(model.NewPassword))
                throw new AppException(_stringLocalizer[MessageType.PasswordNewEmpty]);

            if (model.NewPassword != model.NewPasswordConfirm)
                throw new AppException(_stringLocalizer[MessageType.PasswordNewNotMatch]);

            account.PasswordHash = BC.HashPassword(model.NewPassword);

            account.Updated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public async Task<Account> UploadProfileImage(int id, ProfileImage profileImage)
        {
            try
            {
                var account = GetAccount(id);
                account.Image = profileImage.ImageData;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return account;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new AppException(_stringLocalizer[MessageType.ProfileImageUploadFailed]);
            }
        }

        public ProfileImage GetProfileImage(int id)
        {
            var account = GetAccount(id);
            if (account == null) throw new KeyNotFoundException(_stringLocalizer[MessageType.AccountNotFound]);
            var profileImage = new ProfileImage
            {
                Id = id,
                ImageData = account.Image
            };

            return profileImage;
        }

        public async Task<AppInfoData> StoreAppInfo(AppInfoData appInfoData)
        {
            var existingAppInfos = _context.AppInfoDatas.SingleOrDefault(x => x.AccountId == appInfoData.AccountId && x.Platform == appInfoData.Platform && x.Model == appInfoData.Model && x.OSVersion == appInfoData.OSVersion && x.AppVersion == appInfoData.AppVersion);

            if (existingAppInfos == null)
            {
                // store the provided app info
                _context.AppInfoDatas.Add(appInfoData);
                await _context.SaveChangesAsync();
                return appInfoData;
            }
            else
            {
                // update the existing record by setting the Last Accessed date
                existingAppInfos.LastAccessed = appInfoData.LastAccessed;
                existingAppInfos.Locale = appInfoData.Locale;
                existingAppInfos.Timezone = appInfoData.Timezone;
                _context.AppInfoDatas.Update(existingAppInfos);
                await _context.SaveChangesAsync();
                return existingAppInfos;
            }
        }

        public async Task<PushNotificationTokenData> StorePushNotificationToken(PushNotificationTokenData pushNotificationTokenData)
        {
            var existingPushNotificationToken = _context.PushNotificationTokenDatas.SingleOrDefault(x => x.AccountId == pushNotificationTokenData.AccountId && x.Platform == pushNotificationTokenData.Platform && x.Token == pushNotificationTokenData.Token);

            if (existingPushNotificationToken == null)
            {
                // store the provided push notification token
                _context.PushNotificationTokenDatas.Add(pushNotificationTokenData);
                await _context.SaveChangesAsync();
                return pushNotificationTokenData;
            }
            else
            {
                // update the existing record by setting the Last Accessed date
                existingPushNotificationToken.LastUsed = pushNotificationTokenData.LastUsed;
                await _context.SaveChangesAsync();
                return existingPushNotificationToken;
            }
        }

        public async Task<UserQueryData> StoreUserQuery(UserQueryData userQueryData)
        {
            // store the user query
            _context.UserQueryDatas.Add(userQueryData);
            await _context.SaveChangesAsync();
            return userQueryData;
        }

        public OnboardingAccessResponse RequestOnboardingAccess(OnboardingAccessRequest request)
        {
            ConfirmedSensorPurchaseUserData confirmedSensorPurchaseUserData =  _context.ConfirmedSensorPurchaseUsers.Where(_ => _.Email == request.Email).FirstOrDefault();
            if (confirmedSensorPurchaseUserData != null)
            {
                if (confirmedSensorPurchaseUserData.OnboardingAccessToken == null
                    || confirmedSensorPurchaseUserData.OnboardingAccessDateTime == null
                    || confirmedSensorPurchaseUserData.OnboardingAccessDateTime < DateTime.UtcNow.AddMinutes(-30))
                {
                    // Either no token has been generated or it has expired, so generate a token and email it to the user.
                    confirmedSensorPurchaseUserData.OnboardingAccessToken = RandomTokenString();
                    confirmedSensorPurchaseUserData.OnboardingAccessDateTime = DateTime.UtcNow;
                    _context.SaveChanges();

                    // send email
                    SendOnboardingVerificationEmail(request.Email, confirmedSensorPurchaseUserData.OnboardingAccessToken);
                }
               
                return new OnboardingAccessResponse
                {
                    accessStatus = OnboardingAccessType.PendingVerification
                };
            }

            return new OnboardingAccessResponse
            {
                accessStatus = OnboardingAccessType.Denied
            };

        }

        public OnboardingAccessResponse VerifyOnboardingAccess(VerifyOnboardingAccessRequest request)
        {
            ConfirmedSensorPurchaseUserData confirmedSensorPurchaseUserData = _context.ConfirmedSensorPurchaseUsers.Where(_ => _.Email == request.Email).FirstOrDefault();

            if (confirmedSensorPurchaseUserData != null)
            {
                if (confirmedSensorPurchaseUserData.OnboardingAccessToken == request.Token)
                {
                    // The token matches
                    if (confirmedSensorPurchaseUserData.OnboardingAccessDateTime < DateTime.UtcNow.AddMinutes(-30))
                    {
                        // The token has expired (its over 30 minutes since the token was generated)
                        return new OnboardingAccessResponse
                        {
                            accessStatus = OnboardingAccessType.TokenExpired
                        };
                    }
                    else
                    {
                        // Successfully verified, clear the token so it can't be used multiple times.
                        confirmedSensorPurchaseUserData.OnboardingAccessToken = null;
                        confirmedSensorPurchaseUserData.OnboardingAccessDateTime = null;
                        confirmedSensorPurchaseUserData.VerifiedDateTime = DateTime.UtcNow;
                        _context.SaveChanges();

                        // The token is valid
                        return new OnboardingAccessResponse
                        {
                            accessStatus = OnboardingAccessType.Granted
                        };
                    }
                   
                }
                else
                {
                    // The token doesn't match
                    return new OnboardingAccessResponse
                    {
                        accessStatus = OnboardingAccessType.InvalidToken
                    };
                }
            }

            // No records in database for given email address.  Therefore no verified subscription purchase has been made
            return new OnboardingAccessResponse
            {
                accessStatus = OnboardingAccessType.Denied
            };
        }



        // helper methods

        private Account GetAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) throw new KeyNotFoundException(_stringLocalizer[MessageType.AccountNotFound]);
            return account;
        }

        private (RefreshToken, Account) GetRefreshToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null)
            {
                throw new AppException(_stringLocalizer[MessageType.InvalidToken]);
            }
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new AppException(_stringLocalizer[MessageType.InvalidToken]);
            }
            return (refreshToken, account);
        }

        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "MiBoKo",
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                //commented out for load testing
                //Expires = DateTime.UtcNow.AddMinutes(15),
                Expires = TokenSettings.JsonWebTokenExpiresIn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = TokenSettings.RefreshTokenExpiresIn,
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void SendVerificationEmail(Account account, string origin, bool isEmailChangeRequest = false)
        {
            // origin => Base url => https://localhost:5001
            // origin is null when testing API with Postman
            // origin is not null when testing API with swagger

            string message;

            if (string.IsNullOrEmpty(origin))
            {
#if DEBUG
                if (_launchSettingsAppUrls != null && !string.IsNullOrEmpty(_launchSettingsAppUrls[0]))
                {
                    origin = _launchSettingsAppUrls[0];
                }
                else
                {
                    _logger.LogWarning("AccountService::SendVerificationEmail() => origin is null or empty and could not be retrieved from launchSettings.json!");

                    message = $@"<p>{_stringLocalizer["VerificationMailMessage2"]}</p>
                             <p><code>{account.VerificationToken}</code></p>";
                }
#else
                if (string.IsNullOrEmpty(_origin))
                {
                    return;
                }

                origin = _origin;
                //_logger.LogCritical("AccountService::SendVerificationEmail() => origin is null or empty! No verification email has been sent.");
#endif
            }

            string receiver;
            string subject;
            string verifyUrl;
            string header;

            if (isEmailChangeRequest)
            {
                verifyUrl = $"{origin}/accounts/verify-new-email?token={account.VerificationToken}";
                receiver = account.NewEmail;
                subject = _stringLocalizer["VerificationNewMailSubject"];
                message = $@"<p>{_stringLocalizer["VerificationNewMailMessage"]}</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
                header = $@"{_stringLocalizer["VerificationNewMailHeader"]}
                         {message}";
            }
            else
            {
                verifyUrl = $"{origin}/accounts/verify-email?token={account.VerificationToken}";
                receiver = account.Email;
                subject = _stringLocalizer["VerificationMailSubject"];
                message = $@"<p>{_stringLocalizer["VerificationMailMessage1"]}</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
                header = $@"{_stringLocalizer["VerificationMailHeader"]}
                         {message}";
            }

            _emailService.Send(
                to: receiver,
                subject: subject,
                html: header
            );
        }

        private void SendOnboardingVerificationEmail(string email, string token)
        {
            string message;

            string receiver;
            string subject;
            string verifyUrl;
            string header;

            verifyUrl = $"https://codup-miboko.azurewebsites.net/onboarding/access?token={token}";
            receiver = email;
            subject = _stringLocalizer["VerificationMailSubject"];
            message = $@"<p>{_stringLocalizer["OnboardingAccessVerificationMailMessage1"]}</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            header = $@"{_stringLocalizer["OnboardingAccessVerificationMailHeader"]}
                        {message}";


            _emailService.Send(
                to: receiver,
                subject: subject,
                html: header
            );
        }

        private void SendAlreadyRegisteredEmail(string email, string origin)
        {
            if (string.IsNullOrEmpty(origin))
            {
                origin = _origin;
            }

            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>{_stringLocalizer["ForgotPasswordMessage1", origin]}</p>";
            else
                message = $@"<p>{_stringLocalizer["ForgotPasswordMessage2"]}</p>";

            _emailService.Send(
                to: email,
                subject: _stringLocalizer["ForgotPasswordSubject"],
                html: $@"{_stringLocalizer["ForgotPasswordHeader", email]}
                         {message}"
            );
        }

        private void SendPasswordResetEmail(Account account, string origin)
        {
            if (string.IsNullOrEmpty(origin))
            {
                origin = _origin;
            }

            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/accounts/validate-reset-token?token={account.ResetToken}";
                message = $@"<p>{_stringLocalizer["ResetPasswordMessage1"]}</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>{_stringLocalizer["ResetPasswordMessage2"]}</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: _stringLocalizer["ResetPasswordSubject"],
                html: $@"{_stringLocalizer["ResetPasswordHeader"]}
                         {message}"
            );
        }

        private void CleanUpRefreshTokens(Account account, int validTokenLimit = 9)
        {
            account.RefreshTokens.RemoveAll(x => x.IsActive == false);

            if (account.RefreshTokens.Count > validTokenLimit)
            {
                var sorted = account.RefreshTokens.OrderByDescending(x => x.Created);
                var keepAlive = sorted.Take(validTokenLimit);

                account.RefreshTokens.RemoveAll(x => keepAlive.Contains(x) == false);
            }
        }
    }
}