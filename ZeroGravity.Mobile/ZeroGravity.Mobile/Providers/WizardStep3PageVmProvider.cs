using System.Threading;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class WizardStep3PageVmProvider : PageVmProviderBase, IWizardStep3PageVmProvider
    {
        private readonly IProfileImageService _profileImageService;
        private readonly IMediaService _mediaService;

        public WizardStep3PageVmProvider(ITokenService tokenService, IProfileImageService profileImageService, IMediaService mediaService) : base(tokenService)
        {
            _profileImageService = profileImageService;
            _mediaService = mediaService;
        }

        public bool IsPickPhotoSupported => _mediaService.IsPickPhotoSupported;

        public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            return await _mediaService.PickPhotoAsync(options);
        }

        public async Task<ApiCallResult<bool>> UploadProfilePictureAsync(byte[] imageDaBytes, CancellationToken cancellationToken)
        {
            var apiCallResult = await _profileImageService.UploadImage(imageDaBytes, cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<bool>.Ok(true);
            }

            return ApiCallResult<bool>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<byte[]>> GetProfilePictureAsync(CancellationToken cancellationToken)
        {
            var apiCallResult = await _profileImageService.GetImage(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<byte[]>.Ok(apiCallResult.Value.ImageData);
            }

            return ApiCallResult<byte[]>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}