using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    public class PasswordForgotService : IPasswordForgotService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;

        public PasswordForgotService(ILoggerFactory loggerFactory, IApiService apiService)
        {
            _apiService = apiService;
            _logger = loggerFactory?.CreateLogger<PasswordForgotService>() ?? new NullLogger<PasswordForgotService>();
        }

        public async Task<PasswordForgotResult> RequestForgetLinkAsync(string email, CancellationToken cancellationToken)
        {
            var model = new PasswordForgetModel
            {
                Email = email
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/forgot-password";
            var url = baseUrl + api;

            PasswordForgotResponse response;

            try
            {
                response = await _apiService.PostJsonAsyncRx<PasswordForgetModel, PasswordForgotResponse>(url, model, cancellationToken);
            }
            catch (TaskCanceledException taskCanceledException)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to change password.");
                    return PasswordForgotResult.Error(taskCanceledException.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished password change request cancelled by user.");
                return PasswordForgotResult.Error(taskCanceledException.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return PasswordForgotResult.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return PasswordForgotResult.Error(AppResources.Common_Error_Unknown, ErrorReason.Other);
            }

            return PasswordForgotResult.Ok(response.Message.Value);
        }
    }
}
