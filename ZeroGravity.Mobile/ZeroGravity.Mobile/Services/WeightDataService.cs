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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class WeightDataService : IWeightDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public WeightDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<WeightDataService>() ?? new NullLogger<WeightDataService>();
        }

        public async Task<ApiCallResult<WeightDto>> AddWeightDataAsync(WeightDto weightDataDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/WeightData/create";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PostJsonAsyncRx<WeightDto, WeightDto>(url, weightDataDto, cancellationToken);

                return ApiCallResult<WeightDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateFastingData.");
                    return ApiCallResult<WeightDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WeightDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WeightDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WeightDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<WeightDetailsDto>> AddWeightDetailsDataAsync(WeightDetailsDto proxy, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/create";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PostJsonAsyncRx<WeightDetailsDto, WeightDetailsDto>(url, proxy, cancellationToken);

                return ApiCallResult<WeightDetailsDto>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateFastingData.");
                    return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WeightDetailsDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WeightDetailsDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<WeightDetailsDto>> UpdateAsync(WeightDetailsDto weightDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/update";
            var url = baseUrl + api;

            try
            {
                var weightDetailsDto = await _apiService.PutJsonAsyncRx<WeightDetailsDto, WeightDetailsDto>(url, weightDto, cancellationToken);

                return ApiCallResult<WeightDetailsDto>.Ok(weightDetailsDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to Updating Sugar beat alert Data.");
                    return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WeightDetailsDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WeightDetailsDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<WeightDto>>> GetAll(int accountId, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/GetAll/" + accountId;
            var url = baseUrl + api;

            try
            {
                var weightDtos = await _apiService.GetSingleJsonAsync<List<WeightDto>>(url, cancellationToken);

                return ApiCallResult<List<WeightDto>>.Ok(weightDtos);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetPersonalData.");
                    return ApiCallResult<List<WeightDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<WeightDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<WeightDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<WeightDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<WeightDetailsDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/" + id;
            var url = baseUrl + api;

            try
            {
                var weightDetailsDto = await _apiService.GetSingleJsonAsync<WeightDetailsDto>(url, cancellationToken);

                return ApiCallResult<WeightDetailsDto>.Ok(weightDetailsDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetPersonalData.");
                    return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WeightDetailsDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WeightDetailsDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<WeightDetailsDto>> GetCurrentWeightTrackerAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            var baseUrl = Common.ServerUrl;
            var api = "/WeightTracker/Current/" + accountId;
            var url = baseUrl + api;

            try
            {
                var weightDto = await _apiService.GetSingleJsonAsync<WeightDetailsDto>(url, cancellationToken);

                return ApiCallResult<WeightDetailsDto>.Ok(weightDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetPersonalData.");
                    return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<WeightDetailsDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<WeightDetailsDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<WeightDetailsDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}