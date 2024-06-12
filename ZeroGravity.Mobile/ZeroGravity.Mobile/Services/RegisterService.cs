using System;
using System.Collections.Generic;
using System.Text;
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
    public class RegisterService : IRegisterService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;

        public RegisterService(ILoggerFactory loggerFactory, IApiService apiService)
        {
            _apiService = apiService;
            _logger = loggerFactory?.CreateLogger<RegisterService>() ?? new NullLogger<RegisterService>();
        }

        public async Task<RegisterResult> RegisterAsync(string email, string password, string confirmPassword, bool acceptTerms, bool wantsNewsletter, CancellationToken cancellationToken)
        {
            var registerModel = new RegisterModel
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                AcceptTerms = acceptTerms,
                WantsNewsletter = wantsNewsletter
            };

            var baseUrl = Common.ServerUrl;
            var api = "/accounts/register";
            var url = baseUrl + api;

            string msg;
            try
            {
                var response = await _apiService.PostJsonAsyncRx<RegisterModel, RegisterResponse>(url, registerModel, cancellationToken);
                msg = response.Message.Value;
            }
            catch (TaskCanceledException taskCanceledException)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to register.");
                    return RegisterResult.Error(taskCanceledException.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished register request cancelled by user.");
                return RegisterResult.Error(taskCanceledException.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return RegisterResult.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return RegisterResult.Error(AppResources.Common_Error_Unknown, ErrorReason.Other);
            }

            return RegisterResult.Ok(msg);
        }
    }
}
