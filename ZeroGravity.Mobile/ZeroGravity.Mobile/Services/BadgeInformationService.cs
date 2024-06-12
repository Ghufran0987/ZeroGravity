using System;
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
    public class BadgeInformationService : IBadgeInformationService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public BadgeInformationService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<BadgeInformationService>() ?? new NullLogger<BadgeInformationService>();
        }

        public async Task<ApiCallResult<ActivityBadgeInformationDto>> GetActivityBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/badgeinformation/activity/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var activityBadgeInformationDto = await _apiService.GetSingleJsonAsync<ActivityBadgeInformationDto>(url, cancellationToken);

                return ApiCallResult<ActivityBadgeInformationDto>.Ok(activityBadgeInformationDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetActivityBadgeInformation.");
                    return ApiCallResult<ActivityBadgeInformationDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<ActivityBadgeInformationDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<ActivityBadgeInformationDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ActivityBadgeInformationDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<LiquidIntakeBadgeInformationDto>> GetLiquidIntakeBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/badgeinformation/liquid/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var liquidIntakeBadgeInformationDto = await _apiService.GetSingleJsonAsync<LiquidIntakeBadgeInformationDto>(url, cancellationToken);

                return ApiCallResult<LiquidIntakeBadgeInformationDto>.Ok(liquidIntakeBadgeInformationDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetLiquidIntakeBadgeInformation.");
                    return ApiCallResult<LiquidIntakeBadgeInformationDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<LiquidIntakeBadgeInformationDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<LiquidIntakeBadgeInformationDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<LiquidIntakeBadgeInformationDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MealsBadgeInformationDto>> GetMealsBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/badgeinformation/meals/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var mealsBadgeInformationDto = await _apiService.GetSingleJsonAsync<MealsBadgeInformationDto>(url, cancellationToken);

                return ApiCallResult<MealsBadgeInformationDto>.Ok(mealsBadgeInformationDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetMealsBadgeInformationByDate.");
                    return ApiCallResult<MealsBadgeInformationDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MealsBadgeInformationDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MealsBadgeInformationDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MealsBadgeInformationDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MyDayBadgeInformationDto>> GetMyDayBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/badgeinformation/myday/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var myDayBadgeInformationDto = await _apiService.GetSingleJsonAsync<MyDayBadgeInformationDto>(url, cancellationToken);

                return ApiCallResult<MyDayBadgeInformationDto>.Ok(myDayBadgeInformationDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetMyDayBadgeInformation.");
                    return ApiCallResult<MyDayBadgeInformationDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MyDayBadgeInformationDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MyDayBadgeInformationDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MyDayBadgeInformationDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}