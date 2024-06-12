using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services
{
    public class SugarBeatAlertService : ISugarBeatAlertService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public SugarBeatAlertService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<SugarBeatAlertService>() ?? new NullLogger<SugarBeatAlertService>();
        }

        public async Task<ApiCallResult<SugarBeatAlertDto>> AddAsync(SugarBeatAlertDto alertDto, CancellationToken cancellationToken,bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatalertdata/create";
            var url = baseUrl + api;

            try
            {
                alertDto.AccountId = accountId;

                var result = await _apiService.PostJsonAsyncRx<SugarBeatAlertDto, SugarBeatAlertDto>(url, alertDto, cancellationToken);

                return ApiCallResult<SugarBeatAlertDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateFastingData.");
                    return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatAlertDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatAlertDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<SugarBeatAlertDto>>> GetAlertForPeriodAsync(DateTime fromDate, DateTime toDate, AlertCode? alertCode, CRCCodes? criticalCode,CancellationToken cancellationToken)
        {

            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var from = DateTimeHelper.ToUniversalControllerDate(fromDate, false);
            var to = DateTimeHelper.ToUniversalControllerDate(toDate, false);

            var api = $"/sugarbeatalertata/{accountId}/{from}/{to}/{alertCode}/{criticalCode}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<SugarBeatAlertDto>(url, cancellationToken);

                return ApiCallResult<List<SugarBeatAlertDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetAlertForPeriodAsync.");
                    return ApiCallResult<List<SugarBeatAlertDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at GetAlertForPeriodAsync.");
                return ApiCallResult<List<SugarBeatAlertDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<SugarBeatAlertDto>>.Error(ex.CustomErrorMessage + " at GetAlertForPeriodAsync");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<SugarBeatAlertDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatAlertDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatalertdata/" + id;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<SugarBeatAlertDto>(url, cancellationToken);

                return ApiCallResult<SugarBeatAlertDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetByIdAsync for sugar beat alert.");
                    return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatAlertDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatAlertDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<SugarBeatAlertDto>> UpdateAsync(SugarBeatAlertDto alertDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatalertdata/update";
            var url = baseUrl + api;

            try
            {
                alertDto.AccountId = accountId;

                var result = await _apiService.PutJsonAsyncRx<SugarBeatAlertDto, SugarBeatAlertDto>(url, alertDto, cancellationToken);

                return ApiCallResult<SugarBeatAlertDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating Sugar beat alert Data.");
                    return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatAlertDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatAlertDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatAlertDto>.Error(e.Message);
            }
        }
    }
}
