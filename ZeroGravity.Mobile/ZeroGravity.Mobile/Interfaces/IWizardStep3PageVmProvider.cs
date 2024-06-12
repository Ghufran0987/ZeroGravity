using System.Threading;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWizardStep3PageVmProvider : IPageVmProvider
    {
        bool IsPickPhotoSupported { get; }
        Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);

        Task<ApiCallResult<bool>> UploadProfilePictureAsync(byte[] imageDaBytes, CancellationToken cancellationToken);

        Task<ApiCallResult<byte[]>> GetProfilePictureAsync(CancellationToken cancellationToken);
    }
}