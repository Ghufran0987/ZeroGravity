using System.Threading;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMediaService
    {
        IMedia Current { get; }

        bool IsPickPhotoSupported { get; }
        Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);


        bool IsPickVideoSupported { get; }
        Task<MediaFile> PickVideoAsync(CancellationToken cancellationToken = new CancellationToken());

        bool IsTakeVideoSupported { get; }
        Task<MediaFile> TakeVideoAsync(StoreVideoOptions videoOptions = null, CancellationToken cancellationToken = new CancellationToken());
    }
}
