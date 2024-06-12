using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ActivityDataService : IActivityDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public ActivityDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<ActivityDataService>() ?? new NullLogger<ActivityDataService>();
        }

        public async Task<ApiCallResult<ActivityDataDto>> GetActivityDataAsync(int activityId, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/activity/" + activityId;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<ActivityDataDto>(url, cancellationToken);

                return ApiCallResult<ActivityDataDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetActivityData.");
                    return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<ActivityDataDto>> GetActivityDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/activity/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<ActivityDataDto>(url, cancellationToken);

                return ApiCallResult<ActivityDataDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetActivityDataByDate.");
                    return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
           
        }


        public async Task<ApiCallResult<ActivityDataDto>> CreateActivityDataAsync(ActivityDataDto activityDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/activity/create";
            var url = baseUrl + api;

            try
            {
                activityDataDto.AccountId = accountId;

                var activityDataResult = await _apiService.PostJsonAsyncRx<ActivityDataDto, ActivityDataDto>(url, activityDataDto, cancellationToken);

                return ApiCallResult<ActivityDataDto>.Ok(activityDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateActivityData.");
                    return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<ActivityDataDto>> SyncActivityDataAsync(ActivityDataDto activityDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/activity/sync";
            var url = baseUrl + api;

            try
            {
                activityDataDto.AccountId = accountId;

                var activityDataResult = await _apiService.PostJsonAsyncRx<ActivityDataDto, ActivityDataDto>(url, activityDataDto, cancellationToken);

                return ApiCallResult<ActivityDataDto>.Ok(activityDataResult);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<List<ActivityDataDto>>> GetActivitiesByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/activity/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<List<ActivityDataDto>>(url, cancellationToken);

                return ApiCallResult<List<ActivityDataDto>>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetActivityDataByDate.");
                    return ApiCallResult<List<ActivityDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<ActivityDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<ActivityDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<ActivityDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<ActivityDataDto>> UpdateActivityDataAsync(ActivityDataDto activityDataDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/activity/update";
            var url = baseUrl + api;

            try
            {
                var activityDataResult = await _apiService.PutJsonAsyncRx<ActivityDataDto, ActivityDataDto>(url, activityDataDto, cancellationToken);

                return ApiCallResult<ActivityDataDto>.Ok(activityDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateActivityData.");
                    return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<ActivityDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}