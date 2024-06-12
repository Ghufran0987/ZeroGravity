using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class EducationalInfoProvider : PageVmProviderBase, IEducationalInfoProvider
    {

        private readonly ILogger _logger;
        private readonly IApiService _apiService;
        private readonly ISecureStorageService _secureStorageService;

        public EducationalInfoProvider(ILoggerFactory loggerFactory, ITokenService tokenService, 
            IApiService apiService, ISecureStorageService secureStorageService) : base(tokenService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<EducationalInfoProvider>() ??
                      new NullLogger<EducationalInfoProvider>();
        }

        public async Task<ApiCallResult<EducationalInfoDto>> GetEducationalInfoByIdAsync(CancellationToken cancellationToken,string category)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/Educationalinfo/" + category;
            var url = baseUrl + api;

            try
            {
                var educationalInfoDto = await _apiService.GetSingleJsonAsync<EducationalInfoDto>(url, cancellationToken);

                return ApiCallResult<EducationalInfoDto>.Ok(educationalInfoDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFastingSetting.");
                    return ApiCallResult<EducationalInfoDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<EducationalInfoDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<EducationalInfoDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<EducationalInfoDto>.Error(e.Message);
            }
        }
    }
}
