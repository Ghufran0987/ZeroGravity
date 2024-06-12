using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISugarBeatConnectPageVmProvider : IPageVmProvider
    {
        Task<SugarBeatAccessDetailsProxy> GetSugarBeatAccessDetailsAsync();
        Task SaveSugarBeatAccessDetailsAsync(SugarBeatAccessDetailsProxy proxy);
     
        // von SugarBeatMainPageVmProvider
       // Task<BleInitResult> InitBle();
        bool IsBluetoothOn { get; }
        bool IsDevicePaired { get; }

        bool CheckSugarbeatConnected();
        Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission;
        Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address,string name, int scanTimeout = 10000);
        Task StopScanAsync();
        Task CleanUpAsync(bool IsDiconnectedManually = false);

        // von SugarBeatScanPageVmProvider
        Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken);
        Task<bool> InitAllCharacteristicAsync(SugarBeatDevice device, CancellationToken cancellationToken);
        Task<bool> WritePasskeyAuthenticationAsync(SugarBeatDevice device, string passkeyAuthentication, CancellationToken cancellationToken);
        Task<bool> ReadPasskeyAuthenticationResponseAsync(SugarBeatDevice device, CancellationToken cancellationToken);
        Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task DisconnectDeviceAsync(SugarBeatDevice device);
        // Task WriteTimeSynchronizationRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken);
        //Task<Object> WriteHistorytGlucoseRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken, uint index);
         Task<ApiCallResult<SugarBeatSessionDto>> GetActiveSugarBeatSessionFromDB(DateTime date, CancellationToken tocken);
        bool CheckHistorySyncDone();
        Task DeleteSugarBeatAccessDetailsAsync();
        Task<bool> ReadPassKeyResponse(SugarBeatDevice sugarBeatDevice, CancellationToken token);
    }
}