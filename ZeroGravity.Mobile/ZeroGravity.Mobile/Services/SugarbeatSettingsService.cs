using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services
{
    public class SugarbeatSettingsService : ISugarBeatSettingsSevice
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public SugarbeatSettingsService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<SugarbeatSettingsService>() ?? new NullLogger<SugarbeatSettingsService>();
        }


        public async Task<ApiCallResult<SugarBeatSettingDto>> AddAsync(SugarBeatSettingDto settingDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatsettingdata/create";
            var url = baseUrl + api;

            try
            {
                settingDto.AccountId = accountId;

                var result = await _apiService.PostJsonAsyncRx<SugarBeatSettingDto, SugarBeatSettingDto>(url, settingDto, cancellationToken);

                return ApiCallResult<SugarBeatSettingDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Creating Sugarbeat settings.");
                    return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSettingDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<SugarBeatSettingDto>> GetByAccountIdAsync( CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatsettingdata/" + accountId;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<SugarBeatSettingDto>(url, cancellationToken);

                return ApiCallResult<SugarBeatSettingDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetByAccountIdAsync for sugar beat settings.");
                    return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSettingDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<SugarBeatSettingDto>> UpdateAsync(SugarBeatSettingDto settingDto, CancellationToken cancellationToken, bool saveChanges = true)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/sugarbeatsettingdata/update";
            var url = baseUrl + api;

            try
            {
                settingDto.AccountId = accountId;

                var result = await _apiService.PutJsonAsyncRx<SugarBeatSettingDto, SugarBeatSettingDto>(url, settingDto, cancellationToken);

                return ApiCallResult<SugarBeatSettingDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating Sugarbeat settings Data.");
                    return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<SugarBeatSettingDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<SugarBeatSettingDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<SugarBeatSettingDto>.Error(e.Message);
            }
        }
    }
}
