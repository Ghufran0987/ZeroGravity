using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class SugarBeatMainPageVmProvider : PageVmProviderBase, ISugarBeatMainPageVmProvider
    {
        private readonly IBleService _bleService;
        private readonly IPermissionService _permissionService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ISugarBeatGlucoseService _sugarBeatGlucoseService;
        public SugarBeatMainPageVmProvider(ITokenService tokenService, IBleService bleService,
            IPermissionService permissionService, ISecureStorageService secureStorageService, ISugarBeatGlucoseService sugarBeatGlucoseService) : base(tokenService)
        {
            _permissionService = permissionService;
            _bleService = bleService;
            _secureStorageService = secureStorageService;
            _sugarBeatGlucoseService = sugarBeatGlucoseService;
        }

        public bool IsBluetoothOn => _bleService.IsBluetoothOn;

        public async Task StopScanAsync()
        {
            await _bleService.StopScanAsync();
        }

        public async Task CleanUpAsync(bool IsDiconnectedManually = false)
        {
            await _bleService.CleanUpAsync(null,IsDiconnectedManually);
        }

        public async Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name,
                   int scanTimeout = 10000)
        {
            return await _bleService.StarScanAsync(cancellationToken, address, name, scanTimeout);
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            return await _permissionService.CheckAndRequestPermissionAsync(permission);
        }

        public async Task<SugarBeatGlucoseProxy> ReadCurrentGlucoseAsync(CancellationToken cancellationToken)
        {
            var model = await _bleService.ReadCurrentGlucoseAsync(cancellationToken);
            return new SugarBeatGlucoseProxy(model);
        }
        public async Task<SugarBeatAlert> ReadAlertAsync(CancellationToken cancellationToken)
        {
            var model = await _bleService.ReadAlertAsync(cancellationToken);
            return model;
        }

        public async Task DeleteSugarBeatAccessDetailsAsync()
        {
            try
            {
                var wasRemoved = await _secureStorageService.Remove(SecureStorageKey.SugarBeatAccessDetails);
                if (wasRemoved)
                {
                    System.Diagnostics.Debug.WriteLine("Deleted sugarBEAT access details from secure storage.");
                }
            }
            catch
            {
                await Task.CompletedTask;
            }
        }

        public async Task<ApiCallResult<List<SugarBeatGlucoseProxy>>> GetGlucoseForPeriodAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {

            var apiCallResult = await _sugarBeatGlucoseService.GetGlucoseForPeriodAsync(fromDate, toDate, cancellationToken);
            return apiCallResult.Success
                ? ApiCallResult<List<SugarBeatGlucoseProxy>>.Ok(ProxyConverter.GetSugarBeatGlucoseProxy(apiCallResult.Value))
                : ApiCallResult<List<SugarBeatGlucoseProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);

        }
      
    }
}