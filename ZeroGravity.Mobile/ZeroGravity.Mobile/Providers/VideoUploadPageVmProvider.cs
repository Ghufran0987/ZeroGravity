using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class VideoUploadPageVmProvider : PageVmProviderBase, IVideoUploadPageVmProvider
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger _logger;

        public VideoUploadPageVmProvider(ILoggerFactory loggerFactory, IPermissionService permissionService, ITokenService tokenService) : base(tokenService)
        {
            _permissionService = permissionService;
            _logger = loggerFactory?.CreateLogger<VideoUploadPageVmProvider>() ?? new NullLogger<VideoUploadPageVmProvider>();
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission
        {
            return await _permissionService.CheckAndRequestPermissionAsync(permission);
        }
    }
}
