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
    public class AnalysisSummaryDataService : IAnalysisSummaryDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public AnalysisSummaryDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _secureStorageService = secureStorageService;
            _apiService = apiService;
            _logger = loggerFactory?.CreateLogger<AnalysisSummaryDataService>() ??
                      new NullLogger<AnalysisSummaryDataService>();
        }

        public async Task<ApiCallResult<AnalysisSummaryDto>> GetAnalysisSummaryDataByDateAsync(DateTime dateTime,
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/AnalysisSummary/{accountId}/{DateTimeHelper.ToUniversalControllerDate(dateTime)}";
            var url = $"{baseUrl}{api}";

            try
            {
                var dto = await _apiService.GetSingleJsonAsync<AnalysisSummaryDto>(url, cancellationToken);

                return ApiCallResult<AnalysisSummaryDto>.Ok(dto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetLiquidIntakeDataByDate.");
                    return ApiCallResult<AnalysisSummaryDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<AnalysisSummaryDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<AnalysisSummaryDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<AnalysisSummaryDto>.Error(e.Message);
            }
        }
    }
}