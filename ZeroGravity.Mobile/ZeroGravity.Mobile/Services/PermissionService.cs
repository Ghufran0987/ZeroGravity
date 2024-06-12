using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class PermissionService : IPermissionService
    {
        public async Task<PermissionStatus> CheckStatusAsync(Permissions.BasePermission permission)
        {
            if (permission == null)
            {
                return PermissionStatus.Unknown;
            }

            return await permission.CheckStatusAsync();
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            try
            {
                if (permission == null)
                {
                    return PermissionStatus.Unknown;
                }

                var status = await permission.CheckStatusAsync();
                if (status != PermissionStatus.Granted)
                {
                    status = await permission.RequestAsync();
                }

                return status;
            }
            catch (Exception ex)
            {
                return PermissionStatus.Unknown;
                System.Diagnostics.Debug.WriteLine(ex.Message + " stack trace: " + ex.StackTrace);
            }
        }
    }
}