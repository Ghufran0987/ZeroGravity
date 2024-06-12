using System;
using System.Collections.Generic;
using System.Text;
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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class AccountSecurityService : IAccountSecurityService
    {
        private readonly ILogger _logger;
        private readonly IApiService _apiService;
        private readonly ISecureStorageService _secureStorageService;

        public AccountSecurityService(ILoggerFactory loggerFactory, IApiService apiService, ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<AccountSecurityService>() ?? new NullLogger<AccountSecurityService>();
        }
        public async Task<ApiCallResult<string>> ChangeEmail(string newEmail, CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var model = new EmailChangeModel()
            {
                NewEmail = newEmail
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/change-email/" + userId;
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PutJsonAsyncRx<EmailChangeModel, EmailChangeResponse>(url, model, cancellationToken);
                return ApiCallResult<string>.Ok(result.Message.Value);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to confirm password.");
                    return ApiCallResult<string>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished email change request cancelled by user.");
                return ApiCallResult<string>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);

            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<string>.Error(ex.Message, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<string>.Error(AppResources.Common_Request_Error, ErrorReason.Other);
            }
        }

        public async Task<ApiCallResult<string>> ChangePassword(string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var model = new PasswordChangeModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/change-password/" + userId;
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PutJsonAsyncRx<PasswordChangeModel, PasswordChangeResponse>(url, model, cancellationToken);
                return ApiCallResult<string>.Ok(result.Message.Value);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to confirm password.");
                    return ApiCallResult<string>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished email change request cancelled by user.");
                return ApiCallResult<string>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);

            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<string>.Error(ex.Message, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<string>.Error(AppResources.Common_Request_Error, ErrorReason.Other);
            }
        }

        public async Task<PasswordConfirmResult> ConfirmPassword(string password, CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var model = new PasswordConfirmModel
            {
                UserId = userId,
                Password = password
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/confirm-password";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PostJsonAsyncRx<PasswordConfirmModel, bool>(url, model, cancellationToken);
                if (result)
                {
                    return PasswordConfirmResult.Ok(string.Empty);
                }

                return PasswordConfirmResult.Error(AppResources.Password_Invalid, ErrorReason.None);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to confirm password.");
                    return PasswordConfirmResult.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished password confirm request cancelled by user.");
                return PasswordConfirmResult.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);

            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return PasswordConfirmResult.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return PasswordConfirmResult.Error(AppResources.Common_Request_Error, ErrorReason.Other);
            }
        }

        public async Task<ApiCallResult<string>> DeleteAccount(CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/" + userId;
            var url = baseUrl + api;

            string msg = string.Empty;
            try
            {
                var response = await _apiService.DeleteAsyncRx<AccountDeleteResponse>(url, cancellationToken);
                msg = response.Message.Value;

                return ApiCallResult<string>.Ok(msg);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to delete account.");
                    return ApiCallResult<string>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished account delete request cancelled by user.");
                return ApiCallResult<string>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);

            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<string>.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<string>.Error(e.Message, ErrorReason.Other);
            }
        }
    }
}
