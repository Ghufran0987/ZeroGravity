using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ZeroGravity.Constants;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.Accounts;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;
using AccountResponse = ZeroGravity.Models.Accounts.AccountResponse;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IStringLocalizer<AccountsController> _stringLocalizer;
        private readonly IMapper _mapper;

        public AccountsController(
            IAccountService accountService,
            IStringLocalizer<AccountsController> stringLocalizer,
            IMapper mapper)
        {
            _accountService = accountService;
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = _accountService.Authenticate(model, IpAddress());
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public ActionResult<AuthenticateResponse> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _accountService.RefreshToken(refreshToken, IpAddress());
            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = _stringLocalizer[MessageType.TokenRequired] });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.Role != Role.Admin)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            _accountService.RevokeToken(token, IpAddress());
            return Ok(new { message = _stringLocalizer[MessageType.TokenRevoked] });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = _stringLocalizer[MessageType.RegistrationSuccessful] });
        }

        
        [HttpPost("requestOnboardingAccess")]
        public ActionResult<OnboardingAccessResponse> RequestOnboardingAccess(OnboardingAccessRequest onboardingAccessRequest)
        {
            OnboardingAccessResponse onboardingAccessResponse = _accountService.RequestOnboardingAccess(onboardingAccessRequest);
            return Ok(onboardingAccessResponse);
        }

        [HttpPost("verifyOnboardingAccess")]
        public ActionResult<OnboardingAccessResponse> VerifyOnboardingAccess(VerifyOnboardingAccessRequest onboardingAccessRequest)
        {
            OnboardingAccessResponse onboardingAccessResponse = _accountService.VerifyOnboardingAccess(onboardingAccessRequest);
            return Ok(onboardingAccessResponse);
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail(string token)
        {
            try
            {
                _accountService.VerifyEmail(token);
                return RedirectToPage("/RegisterWelcome");
            }
            catch (Exception e)
            {
                return RedirectToPage("/Error", new { message = e.Message });
            }
        }

        [HttpGet("verify-new-email")]
        public IActionResult VerifyNewEmail(string token)
        {
            try
            {
                _accountService.VerifyEmail(token, true);
                _accountService.UpdateEmail(token);
                return RedirectToPage("/EmailChanged");
            }
            catch (Exception e)
            {
                return RedirectToPage("/Error", new { message = e.Message });
            }
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new { message = _stringLocalizer[MessageType.ForgotPassword] });
        }

        [HttpGet("validate-reset-token")]
        public IActionResult ValidateResetToken(string token)
        {
            try
            {
                _accountService.ValidateResetToken(token);
                return RedirectToPage("/ResetPassword", new { token = token });
            }
            catch (Exception e)
            {
                return RedirectToPage("/Error", new { message = e.Message });
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<AccountResponse>> GetAll()
        {
            var accounts = _accountService.GetAll();
            return Ok(accounts);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public ActionResult<AccountResponse> GetById(int id)
        {
            // users can get their own account and admins can get any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            var account = _accountService.GetById(id);
            return Ok(account);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public ActionResult<AccountResponse> Create(CreateRequest model)
        {
            var account = _accountService.Create(model);
            return Ok(account);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public ActionResult<AccountResponse> Update(int id, UpdateRequest model)
        {
            // users can update their own account and admins can update any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            // only admins can update role
            if (Account.Role != Role.Admin)
                model.Role = null;

            var account = _accountService.Update(id, model);
            return Ok(account);
        }

        [Authorize]
        [HttpPut("change-password/{id:int}")]
        public ActionResult<AccountResponse> UpdatePassword(int id, PasswordChangeRequest model)
        {
            if (id != Account.Id)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            _accountService.UpdatePassword(id, model);
            return Ok(new { message = _stringLocalizer[MessageType.PasswordChangedSuccessful] });
        }

        [Authorize]
        [HttpPut("updatewizardstate/{id:int}")]
        public ActionResult<AccountResponse> UpdateWizardState(int id, UpdateWizardRequest model)
        {
            // users can update their own account and admins can update any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            var updateSuccessful = _accountService.UpdateWizardState(id, model);

            return Ok(updateSuccessful);
        }

        [Authorize]
        [HttpPost("confirm-password")]
        public IActionResult ConfirmPassword(PasswordConfirmRequest model)
        {
            var isCorrectPassword = _accountService.ConfirmPassword(model);

            if (!isCorrectPassword)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            // users can delete their own account and admins can delete any account
            if (id != Account.Id && Account.Role != Role.Admin)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            _accountService.Delete(id);
            return Ok(new { message = _stringLocalizer[MessageType.AccountDeleted] });
        }

        [Authorize]
        [HttpPut("change-email/{id:int}")]
        public IActionResult UpdateEmailRequest(int id, UpdateEmailRequest model)
        {
            // users can update their own account
            if (id != Account.Id)
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });

            _accountService.NewEmailRequest(id, model, Request.Headers["origin"]);

            return Ok(new { message = _stringLocalizer[MessageType.NewEmailRequestedSuccessful] });
        }

        [Authorize]
        [HttpPost("app-info")]
        public async Task<IActionResult> StoreAppInfoAsync(AppInfoDataDto appInfoDataDto)
        {
            VerifyAccountId(appInfoDataDto?.AccountId);
            var appInfoData = DtoConverter.GetAppInfoData(appInfoDataDto);
            _ = await _accountService.StoreAppInfo(appInfoData).ConfigureAwait(false);
            return Ok("AppInfoStored");
        }

        [Authorize]
        [HttpPost("push-notification-token")]
        public async Task<IActionResult> StorePushNotificationToken(PushNotificationTokenDataDto pushNotificationTokenDataDto)
        {
            VerifyAccountId(pushNotificationTokenDataDto?.AccountId);
            var pushNotificationTokenData = DtoConverter.GetPushNotificationTokenData(pushNotificationTokenDataDto);
            _ = await _accountService.StorePushNotificationToken(pushNotificationTokenData).ConfigureAwait(false);
            return Ok("PushNotificationTokenStored");
        }

        // helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = TokenSettings.RefreshTokenExpiresIn
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];

            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [Authorize]
        [HttpPut("profile-image/{id:int}")]
        public async Task<IActionResult> UpdateProfileImage(int id, ProfileImageDto profileImageDto)
        {
            if (id != Account.Id)
            {
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });
            }

            var profileImage = DtoConverter.GetProfileImage(profileImageDto);

            var account = await _accountService.UploadProfileImage(id, profileImage);
            return Ok(account.Image != null);
        }

        [Authorize]
        [HttpGet("profile-image/{id:int}")]
        public ActionResult<ProfileImageDto> GetProfileImageById(int id)
        {
            if (id != Account.Id)
            {
                return Unauthorized(new { message = _stringLocalizer[MessageType.Unauthorized] });
            }

            var profileImage = _accountService.GetProfileImage(id);

            var profileIMageDto = DtoConverter.GetProfileImageDto(profileImage);

            return Ok(profileIMageDto);
        }
    }
}