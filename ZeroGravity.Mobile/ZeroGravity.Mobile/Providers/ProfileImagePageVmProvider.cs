using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Contract.Models.Notification;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class ProfileImagePageVmProvider : PageVmProviderBase, IProfileImagePageVmProvider
    {
        private readonly INotificationService _notificationService;
        private readonly IProfileImageService _profileImageService;
        private readonly ILogger _logger;

        public ProfileImagePageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, 
            INotificationService notificationService, IProfileImageService profileImageService) : base(tokenService)
        {
            _notificationService = notificationService;
            _profileImageService = profileImageService;
            _logger = loggerFactory?.CreateLogger<ProfileImagePageVmProvider>() ??
                      new NullLogger<ProfileImagePageVmProvider>();
        }

        public async Task<ApiCallResult<byte[]>> GetProfilePictureAsync(CancellationToken cancellationToken)
        {
            var apiCallResult =  await _profileImageService.GetImage(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<byte[]>.Ok(apiCallResult.Value.ImageData);
            }

            return ApiCallResult<byte[]>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken)
        {
            return await _notificationService.GetNotificationsAsync(caneCancellationToken);
        }
    }
}