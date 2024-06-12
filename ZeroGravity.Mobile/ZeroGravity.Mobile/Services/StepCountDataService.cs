using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    public class StepCountDataService : IStepCountDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public StepCountDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<StepCountDataService>() ?? new NullLogger<StepCountDataService>();
        }

        public async Task<ApiCallResult<StepCountDataDto>> GetStepCountDataAsync(int activityId, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/stepcountdata/" + activityId;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<StepCountDataDto>(url, cancellationToken);

                return ApiCallResult<StepCountDataDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetStepCountData.");
                    return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<StepCountDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<StepCountDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<StepCountDataDto>> GetStepCountDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/stepcountdata/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var stepCountDataDto = await _apiService.GetSingleJsonAsync<StepCountDataDto>(url, cancellationToken);

                return ApiCallResult<StepCountDataDto>.Ok(stepCountDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetStepCountDataByDate.");
                    return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<StepCountDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<StepCountDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }


        public async Task<ApiCallResult<StepCountDataDto>> CreateStepCountDataAsync(StepCountDataDto activityDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/stepcountdata/create";
            var url = baseUrl + api;

            try
            {
                activityDataDto.AccountId = accountId;

                var stepCountDataResult = await _apiService.PostJsonAsyncRx<StepCountDataDto, StepCountDataDto>(url, activityDataDto, cancellationToken);

                return ApiCallResult<StepCountDataDto>.Ok(stepCountDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateStepCountData.");
                    return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<StepCountDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<StepCountDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<StepCountDataDto>> UpdateStepCountDataAsync(StepCountDataDto stepCountDataDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/stepcountdata/update";
            var url = baseUrl + api;

            try
            {
                var stepCountDataResult = await _apiService.PutJsonAsyncRx<StepCountDataDto, StepCountDataDto>(url, stepCountDataDto, cancellationToken);

                return ApiCallResult<StepCountDataDto>.Ok(stepCountDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateStepCountData.");
                    return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<StepCountDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<StepCountDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<StepCountDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}