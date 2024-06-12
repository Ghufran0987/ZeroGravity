using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces.Communication
{
    public interface IBleService
    {
        bool IsBluetoothOn { get; }

        Task<BleInitResult> Init();

        bool IsDevicePaired { get; }

        Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name, int scanTimeout = 10000);

        Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken, bool autoconnect = false);

       Task DisconnectDeviceAsync(SugarBeatDevice device = null, bool IsDisconnectedManually = false);

        Task<bool> InitAllCharacteristicAsync(SugarBeatDevice device, CancellationToken cancellationToken);

      Task<bool> WritePasskeyAuthenticationAsync(SugarBeatDevice device, string passkeyAuthentication, CancellationToken cancellationToken);

        Task<bool> ReadPasskeyAuthenticationResponseAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task StopScanAsync();

        Task CleanUpAsync(SugarBeatDevice device = null, bool IsDisconnectedManually = false);

        Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task<SugarBeatGlucose> ReadCurrentGlucoseAsync(SugarBeatDevice device, CancellationToken cancellationToken);

        Task ScanForAvailableDevicesAsync(CancellationToken cancellationToken,
         int scanTimeout = 10000);

        Task<SugarBeatGlucose> ReadCurrentGlucoseAsync(CancellationToken cancellationToken);

        Task<SugarBeatAlert> ReadAlertAsync(CancellationToken cancellationToken);

        //Task<Object> ReadHistorytGlucoseResponseBodyAsync(CancellationToken cancellationToken);

        //Task<Object> ReadHistorytGlucoseResponseHeaderAsync(CancellationToken cancellationToken);

       // Task<Object> WriteHistorytGlucoseRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken, uint index=0, bool concurrentRequest = false);

      //  Task WriteTimeSynchronizationRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken);
        bool CheckSugarbeatConnected();

        bool CheckHistorySyncDone();
        void TryReConnectToDeviceAsync(CancellationToken cts);
    }
}