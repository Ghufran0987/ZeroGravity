using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Interfaces;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IVideoUploadPageVmProvider : IPageVmProvider
    {
        Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission;
    }
}
