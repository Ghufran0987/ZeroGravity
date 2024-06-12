using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISugarBeatScanPageVmProvider : IPageVmProvider
    {
        bool IsBluetoothOn { get; }

        Task StopScanAsync();

        Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name, int scanTimeout = 10000);

        Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task DisconnectDeviceAsync(SugarBeatDevice device);

     //   Task<bool> InitAllCharacteristicAsync(SugarBeatDevice device, CancellationToken cancellationToken);
     
       // Task<bool> WritePasskeyAuthenticationAsync(SugarBeatDevice device, string passkeyAuthentication, CancellationToken cancellationToken);

      //  Task<bool> ReadPasskeyAuthenticationResponseAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task CleanUpAsync(bool IsDiconnectedManually = false);
        Task DeleteSugarBeatAccessDetailsAsync();
        void StartScanForAvailableDevicesAsync(CancellationToken cancellationToken,
        int scanTimeout = 10000);

        Task<SugarBeatAccessDetailsProxy> GetSugarBeatAccessDetailsAsync();
        void TryReConnectToDeviceAsync( CancellationToken cts);
        bool CheckSugarbeatConnected();

        Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission;
    }
}