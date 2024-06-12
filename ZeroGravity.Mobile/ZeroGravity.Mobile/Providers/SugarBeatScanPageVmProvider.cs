using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class SugarBeatScanPageVmProvider : PageVmProviderBase, ISugarBeatScanPageVmProvider
    {
        private readonly IBleService _bleService;
        private readonly ILogger _logger;
        private readonly IPermissionService _permissionService;
        private ISecureStorageService _secureStorageService;

        public SugarBeatScanPageVmProvider(ITokenService tokenService, IBleService bleService,
            ILoggerFactory loggerFactory, IPermissionService permissionService, ISecureStorageService secureStorageService) : base(tokenService)
        {
            _permissionService = permissionService;
            _logger = loggerFactory?.CreateLogger<SugarBeatScanPageVmProvider>() ??
                      new NullLogger<SugarBeatScanPageVmProvider>();
            _bleService = bleService;
            _secureStorageService = secureStorageService;
        }

        public bool IsBluetoothOn => _bleService.IsBluetoothOn;

        public async Task StopScanAsync()
        {
            await _bleService?.StopScanAsync();
        }

        public async Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name,
            int scanTimeout = 10000)
        {
            return await _bleService.StarScanAsync(cancellationToken, address,name, scanTimeout);
        }

        public void StartScanForAvailableDevicesAsync(CancellationToken cancellationToken,
        int scanTimeout = 10000)
        {
            _bleService.ScanForAvailableDevicesAsync(cancellationToken, scanTimeout);
        }

        public async Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.ConnectDeviceAsync(device, cancellationToken);
        }

        public async Task DisconnectDeviceAsync(SugarBeatDevice device)
        {
            await _bleService.DisconnectDeviceAsync(device);
        }

        public async Task CleanUpAsync(bool IsDiconnectedManually = false)
        {
            await _bleService.CleanUpAsync(null, IsDiconnectedManually);
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
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Couldnot delete sugarBEAT access details from secure storage: " + ex.Message);
                await Task.CompletedTask;
            }
        }
        
        public async Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.ReadAlertAsync(device, cancellationToken);
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            return await _permissionService.CheckAndRequestPermissionAsync(permission);
        }


        public async Task<SugarBeatAccessDetailsProxy> GetSugarBeatAccessDetailsAsync()
        {
            try
            {
                var model = await _secureStorageService.LoadObject<SugarBeatAccessDetails>(SecureStorageKey.SugarBeatAccessDetails) ?? new SugarBeatAccessDetails { MacAddress = string.Empty, Password = string.Empty };

                return await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = model.MacAddress,
                    Password = model.Password
                });
            }
            catch
            {
                return await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = string.Empty,
                    Password = string.Empty
                });
            }
        }

        public void TryReConnectToDeviceAsync(CancellationToken cts)
        {
            _bleService.TryReConnectToDeviceAsync(cts);
        }

        public bool CheckSugarbeatConnected()
        {
            return _bleService.CheckSugarbeatConnected();
        }
    }
}