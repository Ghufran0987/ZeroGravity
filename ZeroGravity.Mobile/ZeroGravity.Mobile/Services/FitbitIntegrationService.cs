using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class FitbitIntegrationService : IFitbitIntegrationService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public FitbitIntegrationService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<FitbitIntegrationService>() ??
                      new NullLogger<FitbitIntegrationService>();
        }

        public async Task<ApiCallResult<FitbitAccountDto>> GetFitbitAccountAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/fitbit/" + accountId;
            var url = baseUrl + api;

            try
            {
                var fitbitAccountDto = await _apiService.GetSingleJsonAsync<FitbitAccountDto>(url, cancellationToken);

                return ApiCallResult<FitbitAccountDto>.Ok(fitbitAccountDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFitbitAccount.");
                    return ApiCallResult<FitbitAccountDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FitbitAccountDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FitbitAccountDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FitbitAccountDto>.Error(AppResources.Common_Error_Unknown);
            }
        }


        public async Task<ApiCallResult<FitbitActivityDataDto>> GetFitbitActivitiesAsync(int integrationId, 
            DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;

            var api = $"/fitbit/activities/{accountId}/{integrationId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime, false)}";
            var url = baseUrl + api;

            try
            {
                var fitbitActivityDataDto = await _apiService.GetSingleJsonAsync<FitbitActivityDataDto>(url, cancellationToken);

                return ApiCallResult<FitbitActivityDataDto>.Ok(fitbitActivityDataDto);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FitbitActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FitbitActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}