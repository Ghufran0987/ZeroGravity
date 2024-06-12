using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionStatus> CheckStatusAsync(Permissions.BasePermission permission);
        Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission;
    }
}