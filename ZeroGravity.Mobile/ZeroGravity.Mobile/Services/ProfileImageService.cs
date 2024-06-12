using System;
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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class ProfileImageService : IProfileImageService
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApiService _apiService;
        private readonly ILogger _logger;

        public ProfileImageService(ILoggerFactory loggerFactory, ISecureStorageService secureStorageService, IApiService apiService)
        {
            _logger = loggerFactory?.CreateLogger<ProfileImageService>() ?? new NullLogger<ProfileImageService>();
            _secureStorageService = secureStorageService;
            _apiService = apiService;
        }
        public async Task<ApiCallResult<bool>> UploadImage(byte[] imageData, CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var profileImageDto = new ProfileImageDto
            {
                ImageData = imageData
            };

            var baseUrl = Common.ServerUrl;
            
            var api = "/accounts/profile-image/" + userId;
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.PutJsonAsyncRx<ProfileImageDto, bool>(url, profileImageDto, cancellationToken);

                return ApiCallResult<bool>.Ok(result);
            }
            catch (TaskCanceledException taskCanceledEx)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to upload profile image.");
                    return ApiCallResult<bool>.Error(taskCanceledEx.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished profile image upload request cancelled by user.");
                return ApiCallResult<bool>.Error(taskCanceledEx.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<bool>.Error(ex.CustomErrorMessage, ErrorReason.Other);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<bool>.Error(e.Message, ErrorReason.Other);
            }
        }

        public async Task<ApiCallResult<ProfileImageDto>> GetImage(CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;

            var api = "/accounts/profile-image/" + userId;
            var url = baseUrl + api;

            try
            {
                var profileImageDto = await _apiService.GetSingleJsonAsync<ProfileImageDto>(url, cancellationToken);
                return ApiCallResult<ProfileImageDto>.Ok(profileImageDto);
            }
            catch (TaskCanceledException taskCanceledEx)
            {
                _logger.LogInformation($"{taskCanceledEx.Message}", taskCanceledEx);

                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to load profile image.");
                    return ApiCallResult<ProfileImageDto>.Error(taskCanceledEx.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished profile image load request cancelled by user.");
                return ApiCallResult<ProfileImageDto>.Error(taskCanceledEx.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<ProfileImageDto>.Error(ex.CustomErrorMessage);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<ProfileImageDto>.Error(e.Message);
            }
        }
    }
}
