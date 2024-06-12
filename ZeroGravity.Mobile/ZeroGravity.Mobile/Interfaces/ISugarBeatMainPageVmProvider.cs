using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISugarBeatMainPageVmProvider : IPageVmProvider
    {
      //  Task<BleInitResult> InitBle();
        bool IsBluetoothOn { get; }
        Task StopScanAsync();
        Task CleanUpAsync(bool isDisconnectedManually= false);
        Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name, int scanTimeout = 10000);
        Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission;
        Task<SugarBeatGlucoseProxy> ReadCurrentGlucoseAsync(CancellationToken cancellationToken);
        Task DeleteSugarBeatAccessDetailsAsync();
        Task<SugarBeatAlert> ReadAlertAsync(CancellationToken cancellationToken);
        //Task<Object> ReadHistorytGlucoseResponseBodyAsync(CancellationToken cancellationToken);
        //Task<Object> ReadHistorytGlucoseResponseAsync(CancellationToken cancellationToken);
     //   Task<Object> WriteHistorytGlucoseRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken, uint index = 0);

        Task<ApiCallResult<List<SugarBeatGlucoseProxy>>> GetGlucoseForPeriodAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
    }
}