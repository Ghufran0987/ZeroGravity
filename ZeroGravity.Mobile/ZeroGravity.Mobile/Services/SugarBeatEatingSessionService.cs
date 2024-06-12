using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
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
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services
{
    public class SugarBeatEatingSessionService : ISugarBeatEatingSessionService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public SugarBeatEatingSessionService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<SugarBeatEatingSessionService>() ?? new NullLogger<SugarBeatEatingSessionService>();
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionDto>> AddAsync(SugarBeatEatingSessionDto alertDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
            var api = "/SugarBeatEatingSessionData/create";
            var url = baseUrl + api;

            try
            {
                alertDto.AccountId = accountId;
                var result = await _apiService.PostJsonAsyncRx<SugarBeatEatingSessionDto, SugarBeatEatingSessionDto>(url, alertDto, cancellationToken);
                return ApiCallResult<SugarBeatEatingSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to SugarBeatEatingSession.");
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<SugarBeatEatingSessionDto>>> GetSugarBeatEatingSessionForPeriodAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
            var from = DateTimeHelper.ToUniversalControllerDate(fromDate, false);
            var to = DateTimeHelper.ToUniversalControllerDate(toDate, false);
            var api = $"/SugarBeatEatingSessionData/{accountId}/{from}/{to}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<SugarBeatEatingSessionDto>(url, cancellationToken);

                return ApiCallResult<List<SugarBeatEatingSessionDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetSugarBeatEatingSessionForPeriodAsync.");
                    return ApiCallResult<List<SugarBeatEatingSessionDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at GetSugarBeatEatingSessionForPeriodAsync.");
                return ApiCallResult<List<SugarBeatEatingSessionDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<SugarBeatEatingSessionDto>>.Error(ex.CustomErrorMessage + " at GetSugarBeatEatingSessionForPeriodAsync");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<SugarBeatEatingSessionDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/SugarBeatEatingSessionData/" + id;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<SugarBeatEatingSessionDto>(url, cancellationToken);

                return ApiCallResult<SugarBeatEatingSessionDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetByIdAsync for SugarBeatEatingSession.");
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionDto>> UpdateAsync(SugarBeatEatingSessionDto alertDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
            var api = "/SugarBeatEatingSessionData/update";
            var url = baseUrl + api;

            try
            {
                alertDto.AccountId = accountId;
                var result = await _apiService.PutJsonAsyncRx<SugarBeatEatingSessionDto, SugarBeatEatingSessionDto>(url, alertDto, cancellationToken);
                return ApiCallResult<SugarBeatEatingSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating SugarBeatEatingSession.");
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatEatingSessionDto>.Error(ex.CustomErrorMessage);
                }
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatEatingSessionDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<bool>> GetIsSensorWarmedUp(int sessionId, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
  
            var api = "/SugarBeatSessionData/IsSessionWarmedUp/" + sessionId;
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetSingleJsonAsync<bool>(url, cancellationToken);
                return ApiCallResult<bool>.Ok(result);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<bool>.Error(ex.CustomErrorMessage);
                }
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<bool>.Error(e.Message);
            }
        }
    }
}
