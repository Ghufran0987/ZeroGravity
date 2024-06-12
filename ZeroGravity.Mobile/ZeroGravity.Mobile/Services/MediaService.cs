using System.Threading;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class MediaService : IMediaService
    {
        public IMedia Current => CrossMedia.Current;

        public bool IsPickPhotoSupported => Current.IsPickPhotoSupported;
        public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            return await Current.PickPhotoAsync(options);
        }

        public bool IsPickVideoSupported => Current.IsPickVideoSupported;
        public async Task<MediaFile> PickVideoAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Current.PickVideoAsync(cancellationToken);
        }

        public bool IsTakeVideoSupported => Current.IsTakeVideoSupported;
        public async Task<MediaFile> TakeVideoAsync(StoreVideoOptions videoOptions = null, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Current.TakeVideoAsync(videoOptions, cancellationToken);
        }
    }
}
