using System;
using System.Collections.Generic;
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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class ProgressSummaryDataService : IProgressSummaryDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public ProgressSummaryDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _secureStorageService = secureStorageService;
            _apiService = apiService;
            _logger = loggerFactory?.CreateLogger<ProgressSummaryDataService>() ??
                      new NullLogger<ProgressSummaryDataService>();
        }

        public async Task<ApiCallResult<List<ProgressSummaryDto>>> GetAnalysisSummaryDataByDayAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/ProgressSummary/{accountId}/{DateTimeHelper.ToUniversalControllerDate(dateTime)}";
            var url = $"{baseUrl}{api}";

            try
            {
                var dto = await _apiService.GetAllJsonAsync<ProgressSummaryDto>(url, cancellationToken);

                return ApiCallResult<List<ProgressSummaryDto>>.Ok(dto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetProgressByAccountIdForDay.");
                    return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<ProgressSummaryDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<ProgressSummaryDto>>> GetAnalysisSummaryDataByPeriodAsync(DateTime fromTime, DateTime toTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/ProgressSummary/{accountId}/{DateTimeHelper.ToUniversalControllerDate(fromTime)}/{DateTimeHelper.ToUniversalControllerDate(toTime)}";
            var url = $"{baseUrl}{api}";

            try
            {
                var dto = await _apiService.GetSingleJsonAsync<List<ProgressSummaryDto>>(url, cancellationToken);

                return ApiCallResult<List<ProgressSummaryDto>>.Ok(dto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetProgressByAccountIdForPeriod.");
                    return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<ProgressSummaryDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<ProgressSummaryDto>>.Error(e.Message);
            }
        }
    }
}