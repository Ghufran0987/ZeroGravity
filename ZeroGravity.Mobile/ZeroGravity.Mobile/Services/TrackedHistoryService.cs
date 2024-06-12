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
    public class TrackedHistoryService : ITrackedHistoryService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public TrackedHistoryService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<ActivityDataService>() ?? new NullLogger<ActivityDataService>();
        }

        public async Task<ApiCallResult<List<TrackedHistoryDto>>> GetTrackedHistoryByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/trackedhistory/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var liquidIntakeDataDto = await _apiService.GetSingleJsonAsync<List<TrackedHistoryDto>>(url, cancellationToken);

                return ApiCallResult<List<TrackedHistoryDto>>.Ok(liquidIntakeDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetTrackedHistoryByDateAsync.");
                    return ApiCallResult<List<TrackedHistoryDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<TrackedHistoryDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<TrackedHistoryDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<TrackedHistoryDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}