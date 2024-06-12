using System.Threading;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IProfilePageVmProvider : IPageVmProvider
    {
        bool IsPickPhotoSupported { get; }
        Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);

        Task<ApiCallResult<bool>> UploadProfilePictureAsync(byte[] imageDaBytes, CancellationToken cancellationToken);
        Task<ApiCallResult<byte[]>> GetProfilePictureAsync(CancellationToken cancellationToken);

        Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken token);

    }
}
