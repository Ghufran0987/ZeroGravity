using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
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

namespace ZeroGravity.Mobile.Services
{
    public class MeditationService : IMeditationService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public MeditationService(ILoggerFactory loggerFactory, IApiService apiService, ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<MeditationService>() ?? new NullLogger<MeditationService>();
        }


        public async Task<ApiCallResult<MeditationDataDto>> SaveMeditationDataAsync(MeditationDataDto meditationDataDto, CancellationToken cancellationToken = new CancellationToken())
        {
            var baseUrl = Common.ServerUrl;
            var api = "/meditationdata/create";
            var url = baseUrl + api;

            try
            {
                var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
                meditationDataDto.AccountId = accountId;

                var meditationDataResult = await _apiService.PostJsonAsyncRx<MeditationDataDto, MeditationDataDto>(url, meditationDataDto, cancellationToken);

                return ApiCallResult<MeditationDataDto>.Ok(meditationDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation($"Time out while attempting to {nameof(SaveMeditationDataAsync)}.");
                    return ApiCallResult<MeditationDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MeditationDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MeditationDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MeditationDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }



        public async Task<ApiCallResult<List<MeditationDataDto>>> GetAllMeditationDataForDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/meditationdata/{accountId}/{DateTimeHelper.ToUniversalControllerDate(date)}";
            var url = baseUrl + api;

            try
            {
                var meditationDurationSum = await _apiService.GetSingleJsonAsync<List<MeditationDataDto>>(url, cancellationToken);

                return ApiCallResult<List<MeditationDataDto>>.Ok(meditationDurationSum);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation($"Time out while attempting to {nameof(GetAllMeditationDataForDateAsync)}.");
                    return ApiCallResult<List<MeditationDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<MeditationDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<MeditationDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<MeditationDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}
