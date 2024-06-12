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
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services
{
    public class SugarBeatGlucoseService : ISugarBeatGlucoseService
    {

        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public SugarBeatGlucoseService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<SugarBeatGlucoseService>() ?? new NullLogger<SugarBeatGlucoseService>();
        }

        public async Task<ApiCallResult<SugarBeatGlucoseDto>> AddAsync(SugarBeatGlucoseDto glucoseDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatglucosedata/create";
            var url = baseUrl + api;

            try
            {
                glucoseDto.AccountId = accountId;

                var result = await _apiService.PostJsonAsyncRx<SugarBeatGlucoseDto, SugarBeatGlucoseDto>(url, glucoseDto, cancellationToken);

                return ApiCallResult<SugarBeatGlucoseDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to adding sugar beat glucose.");
                    return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user while adding sugar beat glucose.");
                return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatGlucoseDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message);
            }
        }

        public async  Task<ApiCallResult<List<SugarBeatGlucoseDto>>> GetAllGlucoseForSessionId( int sessionId, CancellationToken cancellationToken, bool isGlucoseNull = false)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;


            var api = $"/sugarbeatglucosedata/{sessionId}/{isGlucoseNull}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<SugarBeatGlucoseDto>(url, cancellationToken);

                return ApiCallResult<List<SugarBeatGlucoseDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetAllGlucoseForSessionId.");
                    return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at GetAllGlucoseForSessionId.");
                return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(ex.CustomErrorMessage + " at GetAllGlucoseForSessionId");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<SugarBeatGlucoseDto>>> GetGlucoseForPeriodAsync( DateTime fromDate, DateTime toDate, CancellationToken cancellationToken, bool isGlucoseNull = false)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var from = DateTimeHelper.ToUniversalControllerDate(fromDate, false);
            var to = DateTimeHelper.ToUniversalControllerDate(toDate, false);
            //{ isGlucoseNull}
            var api = $"/sugarbeatglucosedata/{accountId}/{from}/{to}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<SugarBeatGlucoseDto>(url, cancellationToken);

                return ApiCallResult<List<SugarBeatGlucoseDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetAlertForPeriodAsync.");
                    return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at GetAlertForPeriodAsync.");
                return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(ex.CustomErrorMessage + " at GetAlertForPeriodAsync");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<SugarBeatGlucoseDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatGlucoseDto>> UpdateAsync(SugarBeatGlucoseDto glucoseDto, CancellationToken cancellationToken, bool saveChanges = true)
        {

            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatglucosedata/update";
            var url = baseUrl + api;

            try
            {
                glucoseDto.AccountId = accountId;

                var result = await _apiService.PutJsonAsyncRx<SugarBeatGlucoseDto, SugarBeatGlucoseDto>(url, glucoseDto, cancellationToken);

                return ApiCallResult<SugarBeatGlucoseDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating Sugar beat alert Data.");
                    return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatGlucoseDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatGlucoseDto>.Error(e.Message);
            }
        }
    }
}
