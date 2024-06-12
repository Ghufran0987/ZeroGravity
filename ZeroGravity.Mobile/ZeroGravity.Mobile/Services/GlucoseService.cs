using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
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
    public class GlucoseService : IGlucoseService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public GlucoseService(ILoggerFactory loggerFactory, IApiService apiService, ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;

            _logger = loggerFactory?.CreateLogger<GlucoseService>() ?? new NullLogger<GlucoseService>();
        }

        public async Task<ApiCallResult<GlucoseDataDto>> SaveGlucoseAsync(GlucoseDataDto glucoseDataDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/glucosedata/create";
            var url = baseUrl + api;

            try
            {
                var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
                glucoseDataDto.AccountId = accountId;

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var glucoseDataResult = await _apiService.PostJsonAsyncRx<GlucoseDataDto, GlucoseDataDto>(url, glucoseDataDto, cancellationToken, settings);

                return ApiCallResult<GlucoseDataDto>.Ok(glucoseDataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation($"Time out while attempting to {nameof(SaveGlucoseAsync)}.");
                    return ApiCallResult<GlucoseDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<GlucoseDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<GlucoseDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<GlucoseDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}
