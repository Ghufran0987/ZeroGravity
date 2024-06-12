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
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class FastingSettingService : IFastingSettingsService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public FastingSettingService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<FastingSettingService>() ?? new NullLogger<FastingSettingService>();
        }

        public async Task<ApiCallResult<FastingSettingDto>> GetFastingSettingByIdAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/fastingsetting/" + accountId;
            var url = baseUrl + api;

            try
            {
                var fastingSettingDto = await _apiService.GetSingleJsonAsync<FastingSettingDto>(url, cancellationToken);

                return ApiCallResult<FastingSettingDto>.Ok(fastingSettingDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFastingSetting.");
                    return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingSettingDto>.Error(e.Message);
            }
        }


        public async Task<ApiCallResult<FastingSettingDto>> CreateFastingSettingAsync(FastingSettingDto fastingSettingDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/fastingsetting/create";
            var url = baseUrl + api;

            try
            {
                fastingSettingDto.AccountId = accountId;

                var fastingSettingResult = await _apiService.PostJsonAsyncRx<FastingSettingDto, FastingSettingDto>(url, fastingSettingDto, cancellationToken);

                return ApiCallResult<FastingSettingDto>.Ok(fastingSettingResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateFastingSetting.");
                    return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingSettingDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<FastingSettingDto>> UpdateFastingSettingAsync(FastingSettingDto fastingSettingDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/fastingsetting/update";
            var url = baseUrl + api;

            try
            {
                var fastingSettingResult = await _apiService.PutJsonAsyncRx<FastingSettingDto, FastingSettingDto>(url, fastingSettingDto, cancellationToken);

                return ApiCallResult<FastingSettingDto>.Ok(fastingSettingResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateFastingSetting.");
                    return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FastingSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FastingSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FastingSettingDto>.Error(e.Message);
            }
        }
    }
}