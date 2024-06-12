using System;
using System.Collections.Generic;
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
    public class IntegrationDataService : IIntegrationDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public IntegrationDataService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<IntegrationDataService>() ?? new NullLogger<IntegrationDataService>();
        }

        public async Task<ApiCallResult<List<IntegrationDataDto>>> GetIntegrationDataListAsync(
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/integrationdata/" + accountId;
            var url = baseUrl + api;

            try
            {
                var integrationDataDtos =
                    await _apiService.GetSingleJsonAsync<List<IntegrationDataDto>>(url, cancellationToken);

                return ApiCallResult<List<IntegrationDataDto>>.Ok(integrationDataDtos);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetIntegrationDataList.");
                    return ApiCallResult<List<IntegrationDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<IntegrationDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<IntegrationDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<IntegrationDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        //public async Task<ApiCallResult<LinkedIntegrationDto>> GetLinkedIntegrationAsync(int integrationId, CancellationToken cancellationToken)
        //{
        //    var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

        //    var baseUrl = Common.ServerUrl;
        //    var api = "/integrationdata/tokendata/" + accountId + "/" + integrationId;
        //    var url = baseUrl + api;

        //    try
        //    {
        //        var linkedIntegrationDto =
        //            await _apiService.GetSingleJsonAsync<LinkedIntegrationDto>(url, cancellationToken);

        //        return ApiCallResult<LinkedIntegrationDto>.Ok(linkedIntegrationDto);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is HttpException ex)
        //        {
        //            _logger.LogInformation($"{e.Message}", e);
        //            return ApiCallResult<LinkedIntegrationDto>.Error(ex.CustomErrorMessage);
        //        }

        //        _logger.LogInformation($"{e.Message}", e);
        //        return ApiCallResult<LinkedIntegrationDto>.Error(AppResources.Common_Error_Unknown);
        //    }
        //}

        public async Task<ApiCallResult<List<IntegrationDataDto>>> GetLinkedIntegrationListAsync(
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/integrationdata/linked/" + accountId;
            var url = baseUrl + api;

            try
            {
                var integrationDataDtos =
                    await _apiService.GetSingleJsonAsync<List<IntegrationDataDto>>(url, cancellationToken);

                return ApiCallResult<List<IntegrationDataDto>>.Ok(integrationDataDtos);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetLinkedIntegrationList.");
                    return ApiCallResult<List<IntegrationDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<IntegrationDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<IntegrationDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<IntegrationDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<bool>> DeleteLinkedIntegrationAsync(int integrationId,
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/integrationdata/" + accountId + "/" + integrationId;
            var url = baseUrl + api;

            try
            {
                await _apiService.DeleteAsync(url, cancellationToken);

                return ApiCallResult<bool>.Ok(true);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to DeleteLinkedIntegration.");
                    return ApiCallResult<bool>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<bool>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<bool>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<bool>.Error(AppResources.Common_Error_Unknown);
            }

        }
    }
}