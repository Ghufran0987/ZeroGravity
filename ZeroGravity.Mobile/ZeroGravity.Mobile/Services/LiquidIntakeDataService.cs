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
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class LiquidIntakeDataService : ILiquidIntakeDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public LiquidIntakeDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<LiquidIntakeDataService>() ??
                      new NullLogger<LiquidIntakeDataService>();
        }

        public async Task<ApiCallResult<LiquidIntakeDto>> GetLiquidIntakeDataAsync(int liquidIntakeId, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/liquidintake/" + liquidIntakeId;
            var url = baseUrl + api;

            try
            {
                var liquidIntakeDataDto = await _apiService.GetSingleJsonAsync<LiquidIntakeDto>(url, cancellationToken);

                return ApiCallResult<LiquidIntakeDto>.Ok(liquidIntakeDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetLiquidIntakeData.");
                    return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<LiquidIntakeDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<LiquidIntakeDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<List<LiquidIntakeDto>>> GetLiquidIntakeDataByDateAsync(DateTime targetDateTime, LiquidType type, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/liquidintake/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}/{(int)type}";
            var url = baseUrl + api;

            try
            {
                var liquidIntakeDataDto = await _apiService.GetSingleJsonAsync<List<LiquidIntakeDto>>(url, cancellationToken);

                return ApiCallResult<List<LiquidIntakeDto>>.Ok(liquidIntakeDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetLiquidIntakeDataByDate.");
                    return ApiCallResult<List<LiquidIntakeDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<LiquidIntakeDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<LiquidIntakeDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<LiquidIntakeDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }


        public async Task<ApiCallResult<LiquidIntakeDto>> CreateLiquidIntakeDataAsync(LiquidIntakeDto liquidIntakeDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/liquidintake/create";
            var url = baseUrl + api;

            try
            {
                liquidIntakeDto.AccountId = accountId;

                var liquidIntakeDataResult =
                    await _apiService.PostJsonAsyncRx<LiquidIntakeDto, LiquidIntakeDto>(url,
                        liquidIntakeDto, cancellationToken);

                return ApiCallResult<LiquidIntakeDto>.Ok(liquidIntakeDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateLiquidIntakeData.");
                    return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<LiquidIntakeDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<LiquidIntakeDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<LiquidIntakeDto>> UpdateLiquidIntakeDataAsync(LiquidIntakeDto liquidIntakeDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/liquidintake/update";
            var url = baseUrl + api;

            try
            {
                var liquidIntakeDataResult =
                    await _apiService.PutJsonAsyncRx<LiquidIntakeDto, LiquidIntakeDto>(url, liquidIntakeDto, cancellationToken);

                return ApiCallResult<LiquidIntakeDto>.Ok(liquidIntakeDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateLiquidIntakeData.");
                    return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<LiquidIntakeDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<LiquidIntakeDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<LiquidIntakeDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}