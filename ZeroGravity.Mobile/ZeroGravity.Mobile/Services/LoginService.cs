using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Services
{
    public class LoginService : ILoginService
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILogger _logger;

        public LoginService(ILoggerFactory loggerFactory, IApiService apiService, ITokenService tokenService, ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _tokenService = tokenService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<LoginService>() ?? new NullLogger<LoginService>();
        }
        public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken, bool saveToken = false)
        {
            var loginModel = new LoginModel
            {
                Email = email,
                Password = password
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/authenticate";
            var url = baseUrl + api;


            LoginResponse response;

            try
            {
                response = await _apiService.PostJsonAsyncRx<LoginModel, LoginResponse>(url, loginModel, cancellationToken);
            }
            catch (TaskCanceledException taskCanceledException)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to login.");
                    return LoginResult.Error(taskCanceledException.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished login request cancelled by user.");
                return LoginResult.Error(taskCanceledException.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return LoginResult.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return LoginResult.Error(AppResources.Login_Error, ErrorReason.Other);
            }

            // save JsonWebToken into SecureStorage
            await _tokenService.AddOrUpdateJwt(response.JwtToken);
            // save userId into SecureStorage
            var userId = response.Id;
            await _secureStorageService.SaveValue(SecureStorageKey.UserId, userId);

            // save user email into SecureStorage
            await _secureStorageService.SaveString(SecureStorageKey.UserEmail, email);

            //save display preferences into DisplayConversionService
            DisplayConversionService.SetDisplayPrefences(response.DateTimeDisplayType, response.UnitDisplayType);

            return LoginResult.Ok();
        }

        public async Task<LoginResult> LoginSilent()
        {
            if (await _tokenService.IsRefreshTokenExpired())
            {
                // no silent login if both, jwt and refreshToken are expired
                // if the refreshToken is expired, the jwt is automatically expired too
                return LoginResult.Error("Both, JwtToken and refreshToken expired.", ErrorReason.TokenExpired);
            }

            // can do silent login (user can proceed without providing credentials again)
            return LoginResult.Ok();
        }
    }
}
