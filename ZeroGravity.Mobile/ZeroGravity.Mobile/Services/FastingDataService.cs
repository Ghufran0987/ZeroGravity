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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class FastingDataService : IFastingDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public FastingDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<FastingDataService>() ?? new NullLogger<FastingDataService>();
        }

        public async Task<ApiCallResult<FastingDataDto>> GetFastingDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/fastingdata/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime, false)}";
            var url = baseUrl + api;

            try
            {
                var fastingDataDto = await _apiService.GetSingleJsonAsync<FastingDataDto>(url, cancellationToken);
                if(fastingDataDto == null)
                {
                    fastingDataDto = new FastingDataDto();
                    fastingDataDto.AccountId = accountId;
                    fastingDataDto.Duration = new TimeSpan(0, 0, 0);
                }
                return ApiCallResult<FastingDataDto>.Ok(fastingDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFastingDataByDate.");
                    return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (NullReferenceException nullRefEx)
            {
                _logger.LogInformation($"{nullRefEx.Message}", nullRefEx);
                return ApiCallResult<FastingDataDto>.Error($"No data found for specified date {targetDateTime.ToShortDateString()}.", ErrorReason.NoData);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingDataDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<IEnumerable<FastingDataDto>>> GetActiveFastingDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var utcTime = DateTimeHelper.ToUniversalControllerDate(targetDateTime, false);
            var api = $"/fastingdata/{accountId}/{utcTime}/active";
            var url = baseUrl + api;

            try
            {
                var fastingDataDto = await _apiService.GetAllJsonAsync<FastingDataDto>(url, cancellationToken);

                return ApiCallResult<IEnumerable<FastingDataDto>>.Ok(fastingDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFastingDataByDate.");
                    return ApiCallResult<IEnumerable<FastingDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<IEnumerable<FastingDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<IEnumerable<FastingDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<IEnumerable<FastingDataDto>>.Error(e.Message);
            }
        }


        public async Task<ApiCallResult<FastingDataDto>> CreateFastingDataAsync(FastingDataDto fastingDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/fastingdata/create";
            var url = baseUrl + api;

            try
            {
                fastingDataDto.AccountId = accountId;

                var fastingDataResult = await _apiService.PostJsonAsyncRx<FastingDataDto, FastingDataDto>(url, fastingDataDto, cancellationToken);

                return ApiCallResult<FastingDataDto>.Ok(fastingDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateFastingData.");
                    return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingDataDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<FastingDataDto>> UpdateFastingDataAsync(FastingDataDto fastingSettingDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/fastingdata/update";
            var url = baseUrl + api;

            try
            {
                var fastingDataResult = await _apiService.PutJsonAsyncRx<FastingDataDto, FastingDataDto>(url, fastingSettingDto, cancellationToken);

                return ApiCallResult<FastingDataDto>.Ok(fastingDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateFastingData.");
                    return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingDataDto>.Error(e.Message);
            }
        }
    }
}