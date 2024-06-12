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
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services
{
  public  class SugarBeatSessionService : ISugarBeatSessionService
    {

        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public SugarBeatSessionService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<SugarBeatSessionService>() ?? new NullLogger<SugarBeatSessionService>();
        }
        public async  Task<ApiCallResult<SugarBeatSessionDto>> AddAsync(SugarBeatSessionDto sessionDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatsessiondata/create";
            var url = baseUrl + api;

            try
            {
                sessionDto.AccountId = accountId;

                var result = await _apiService.PostJsonAsyncRx<SugarBeatSessionDto, SugarBeatSessionDto>(url, sessionDto, cancellationToken);

                return ApiCallResult<SugarBeatSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Create sugar beat session Data.");
                    return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatSessionDto>> GetActiveSessionAsync( DateTime targetDate, CancellationToken cancellationToken, bool includeGlucoseData = false)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var from = DateTimeHelper.ToUniversalControllerDate(targetDate, false);
            var api = $"/SugarBeatSessionData/{accountId}/{from}/active";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetSingleJsonAsync<SugarBeatSessionDto>(url, cancellationToken);

                return ApiCallResult<SugarBeatSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation($"Time out while attempting to {nameof(GetActiveSessionAsync)}.");
                    return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSessionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<SugarBeatSessionDto>> GetByIdAsync(int id, CancellationToken cancellationToken, bool includeGlucoseData = false)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/sugarbeatsessiondata/{id}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetSingleJsonAsync<SugarBeatSessionDto>(url, cancellationToken);

                return ApiCallResult<SugarBeatSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation($"Time out while attempting to {nameof(GetActiveSessionAsync)}.");
                    return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSessionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<List<SugarBeatSessionDto>>> GetSessionForPeriodAsync( DateTime fromDate, DateTime toDate, CancellationToken cancellationToken, bool includeGlucoseData = false)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var from = DateTimeHelper.ToUniversalControllerDate(fromDate, false);
            var to = DateTimeHelper.ToUniversalControllerDate(toDate, false);

            var api = $"/sugarbeatsessiondata/{accountId}/{from}/{to}/{includeGlucoseData}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<SugarBeatSessionDto>(url, cancellationToken);

                return ApiCallResult<List<SugarBeatSessionDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetSessionForPeriodAsync.");
                    return ApiCallResult<List<SugarBeatSessionDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at GetSessionForPeriodAsync.");
                return ApiCallResult<List<SugarBeatSessionDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<SugarBeatSessionDto>>.Error(ex.CustomErrorMessage + " at GetSessionForPeriodAsync");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<SugarBeatSessionDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatSessionDto>> UpdateAsync(SugarBeatSessionDto sessionDto, CancellationToken cancellationToken, bool saveChanges = true)
        {

            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatsessiondata/update";
            var url = baseUrl + api;

            try
            {
                sessionDto.AccountId = accountId;

                var result = await _apiService.PutJsonAsyncRx<SugarBeatSessionDto, SugarBeatSessionDto>(url, sessionDto, cancellationToken);

                return ApiCallResult<SugarBeatSessionDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating Sugar session Data.");
                    return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSessionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSessionDto>.Error(e.Message);
            }
        }
    }
}
