using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Providers
{
    public class SugarBeatConnectPageVmProvider : PageVmProviderBase, ISugarBeatConnectPageVmProvider
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IBleService _bleService;
        private readonly IPermissionService _permissionService;
        private readonly ISugarBeatSessionService _sessionService;
        private readonly ISugarBeatEatingSessionService _eatingSessionService;

        public SugarBeatConnectPageVmProvider(ITokenService tokenService, ISecureStorageService secureStorageService,
            IBleService bleService, IPermissionService permissionService, ISugarBeatSessionService sessionService, ISugarBeatEatingSessionService eatingSessionService) : base(tokenService)
        {
            _secureStorageService = secureStorageService;
            _bleService = bleService;
            _permissionService = permissionService;
            _sessionService = sessionService;
            _eatingSessionService = eatingSessionService;
        }

        public bool CheckHistorySyncDone()
        {
            return _bleService.CheckHistorySyncDone();
        }
        public bool CheckSugarbeatConnected()
        {
            return _bleService.CheckSugarbeatConnected();
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

        public async Task SaveSugarBeatAccessDetailsAsync(SugarBeatAccessDetailsProxy proxy)
        {
            try
            {
                if (proxy == null)
                {
                    await Task.CompletedTask;
                }
                else
                {
                    var model = new SugarBeatAccessDetails { MacAddress = proxy.Address, Password = proxy.Password };
                    await _secureStorageService.SaveObject(SecureStorageKey.SugarBeatAccessDetails, model);
                    System.Diagnostics.Debug.WriteLine("Saved sugarBEAT access details to secure storage.");
                }
            }
            catch
            {
                await Task.CompletedTask;
            }
        }

        public bool IsBluetoothOn => _bleService.IsBluetoothOn;
        public bool IsDevicePaired => _bleService.IsDevicePaired;

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : Permissions.BasePermission
        {
            return await _permissionService.CheckAndRequestPermissionAsync(permission);
        }

        public async Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> address, string name, int scanTimeout = 10000)
        {
            return await _bleService.StarScanAsync(cancellationToken, address, name, scanTimeout);
        }

        public async Task StopScanAsync()
        {
            await _bleService.StopScanAsync();
        }

        public async Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.ConnectDeviceAsync(device, cancellationToken);
        }

        public async Task<bool> InitAllCharacteristicAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.InitAllCharacteristicAsync(device, cancellationToken);
        }

        public async Task<bool> WritePasskeyAuthenticationAsync(SugarBeatDevice device, string passkeyAuthentication, CancellationToken cancellationToken)
        {
            return await _bleService.WritePasskeyAuthenticationAsync(device, passkeyAuthentication, cancellationToken);
        }

        public async Task<bool> ReadPasskeyAuthenticationResponseAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.ReadPasskeyAuthenticationResponseAsync(device, cancellationToken);
        }

        public async Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            return await _bleService.ReadAlertAsync(device, cancellationToken);
        }

        public async Task DisconnectDeviceAsync(SugarBeatDevice device)
        {
            await _bleService.DisconnectDeviceAsync(device);
        }
        public async Task CleanUpAsync(bool IsDiconnectedManually = false)
        {
            await _bleService.CleanUpAsync(null, IsDiconnectedManually);
        }

        public async Task<ApiCallResult<SugarBeatSessionDto>> GetActiveSugarBeatSessionFromDB(DateTime date, CancellationToken tocken)
        {
            return await _sessionService.GetActiveSessionAsync(date, tocken);
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

        public async Task<bool> ReadPassKeyResponse(SugarBeatDevice sugarBeatDevice, CancellationToken token)
        {
            return await _bleService.ReadPasskeyAuthenticationResponseAsync(sugarBeatDevice, token);
        }
    }
}