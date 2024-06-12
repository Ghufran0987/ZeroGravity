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
    public class WellbeingDataService : IWellbeingDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public WellbeingDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<WellbeingDataService>() ?? new NullLogger<WellbeingDataService>();
        }

        public async Task<ApiCallResult<WellbeingDataDto>> GetWellbeingDataAsync(int activityId,
            CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/wellbeingdata/" + activityId;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<WellbeingDataDto>(url, cancellationToken);

                return ApiCallResult<WellbeingDataDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetWellbeingData.");
                    return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WellbeingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WellbeingDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<WellbeingDataDto>> GetWellbeingDataByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/wellbeingdata/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<WellbeingDataDto>(url, cancellationToken);

                return ApiCallResult<WellbeingDataDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetWellbeingDataByDate.");
                    return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WellbeingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WellbeingDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }


        public async Task<ApiCallResult<WellbeingDataDto>> CreateWellbeingDataAsync(WellbeingDataDto activityDataDto,
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/wellbeingdata/create";
            var url = baseUrl + api;

            try
            {
                activityDataDto.AccountId = accountId;

                var activityDataResult =
                    await _apiService.PostJsonAsyncRx<WellbeingDataDto, WellbeingDataDto>(url, activityDataDto,
                        cancellationToken);

                return ApiCallResult<WellbeingDataDto>.Ok(activityDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateWellbeingData.");
                    return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WellbeingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WellbeingDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<WellbeingDataDto>> UpdateWellbeingDataAsync(WellbeingDataDto activityDataDto,
            CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/wellbeingdata/update";
            var url = baseUrl + api;

            try
            {
                var activityDataResult =
                    await _apiService.PutJsonAsyncRx<WellbeingDataDto, WellbeingDataDto>(url, activityDataDto,
                        cancellationToken);

                return ApiCallResult<WellbeingDataDto>.Ok(activityDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateWellbeingData.");
                    return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WellbeingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WellbeingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WellbeingDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}