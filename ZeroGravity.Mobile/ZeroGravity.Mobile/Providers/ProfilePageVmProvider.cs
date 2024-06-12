using System.Threading;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class ProfilePageVmProvider : PageVmProviderBase, IProfilePageVmProvider
    {
        private readonly IMediaService _mediaService;
        private readonly IProfileImageService _profileImageService;
        private readonly IUserDataService _userDataService;


        public ProfilePageVmProvider(IMediaService mediaService, 
            IProfileImageService profileImageService, ITokenService tokenService, IUserDataService userDataService) : base(tokenService)
        {
            _mediaService = mediaService;
            _profileImageService = profileImageService;
            _userDataService = userDataService;
        }


        // Xamarin.Media.Plugin
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

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken token)
        {
            var apiCallResult = await _userDataService.GetAccountDataAsync(token);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}
