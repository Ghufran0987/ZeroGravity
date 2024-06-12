using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Prism.Events;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Services.Communication
{
    public class BleService : IBleService
    {
        private readonly IEventAggregator _eventAggregator;

        private IBluetoothLE _ble;
        private SugarBeatDevice _device;
        private string _deviceName;

        private SugarBeatSessionDto ActiveSession;
        private readonly ILogger _logger;
        private ushort readCount = 0;
        private bool IsSessionExpiryDisplayed = false;
        private DateTime? lastSyncTime = null;

        private readonly ISugarBeatSessionService _sugarBeatSessionService;
        private readonly ISugarBeatAlertService _sugarBeatAlertService;
        private readonly ISugarBeatGlucoseService _sugarBeatGlucosseService;
        private readonly ISugarBeatSettingsSevice _settingsService;
        private readonly ISecureStorageService _secureStorageService;
        private SugarBeatAccessDetailsProxy sugarBeatAccessDetails;

        public BleService(IEventAggregator eventAggregator, ILoggerFactory loggerFactory, ISugarBeatSessionService sugarBeatSessionService, ISugarBeatAlertService sugarBeatAlertService,
            ISugarBeatGlucoseService sugarBeatGlucosseService, ISugarBeatSettingsSevice settingsService, ISecureStorageService secureStorageService
            )
        {
            _eventAggregator = eventAggregator;
            _logger = loggerFactory?.CreateLogger<BleService>() ?? new NullLogger<BleService>();
            _sugarBeatSessionService = sugarBeatSessionService;
            _sugarBeatAlertService = sugarBeatAlertService;
            _sugarBeatGlucosseService = sugarBeatGlucosseService;
            _settingsService = settingsService;
            _secureStorageService = secureStorageService;
            IsHistorySyncDone = false;
        }

        public bool IsBluetoothOn { get; private set; }

        public bool IsDevicePaired { get; private set; }

        public bool IsConnected
        {
            get
            {
                if (_device == null) return false;
                else
                {
                    if (_device.PluginBleDevice?.State == DeviceState.Disconnected || _device.PluginBleDevice?.State == DeviceState.Limited)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool GetIsConnected()
        {
            return IsConnected;
        }

        public bool IsHistorySyncDone
        {
            get; private set;
        }

        #region Glucose

        private async void CurrentGlucoseValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Payload In CurrentGlucoseValueUpdated: " + e.Characteristic.Value);
                string bitString = BitConverter.ToString(e.Characteristic.Value);
                Debug.WriteLine("Payload In CurrentGlucoseValueUpdated:value length " + e.Characteristic.Value.Length + " Value: " + bitString);

                if (IsSessionExpired())
                {
                    return;
                }
                if (e.Characteristic.Value.Length == 19 || e.Characteristic.Value.Length == 17)
                {
                    try
                    {
                        var value = new SugarBeatGlucose(e.Characteristic.Value);
                        var message = $"CurrentGlucoseValueUpdated payload -> {nameof(value.DateTime)}:{value.DateTime}, {nameof(value.Battery)}:{value.Battery}, {nameof(value.ActiveAlarmBitmaps)}:{value.ActiveAlarmBitmaps}, {nameof(value.Glucose)}:{value.Glucose}";
                        _logger.LogInformation(message);

                        if (value != null)
                        {
                            if (await SaveGlucose(value))
                            {
                                _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Publish(value);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, exception.Message);
                        //  _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Publish(default);
                        Debug.WriteLine("Exception In CurrentGlucoseValueUpdated: " + exception.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AdapterDeviceConnected => exception: " + ex.Message);
                _logger.LogCritical(ex, ex.Message);
            }
        }

        public async Task<SugarBeatGlucose> ReadCurrentGlucoseAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            try
            {
                if (device == null) return default;
                var readBytes = await device.Characteristics[CharacteristicType.CurrentGlucose]
                    .ReadAsync(cancellationToken);
                if (readBytes != null) return default;
                if (readBytes.Length <= 0) return default;
                var value = new SugarBeatGlucose(readBytes);
                var message = $"Read CurrentGlucose -> {nameof(value.DateTime)}:{value.DateTime}, {nameof(value.Battery)}:{value.Battery}, {nameof(value.ActiveAlarmBitmaps)}:{value.ActiveAlarmBitmaps}, {nameof(value.Glucose)}:{value.Glucose}";
                _logger.LogInformation(message);
                return value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception In ReadAlertAsync2: " + e.Message);
                _logger.LogCritical(e, e.Message);
                return default;
            }
        }

        public async Task<SugarBeatGlucose> ReadCurrentGlucoseAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_device == null)
                    return default;
                var readBytes = await _device.Characteristics[CharacteristicType.CurrentGlucose]
                    .ReadAsync(cancellationToken);
                if (readBytes != null) return default;
                if (readBytes.Length <= 0) return default;
                var value = new SugarBeatGlucose(readBytes);
                var message = $"Read CurrentGlucose -> {nameof(value.DateTime)}:{value.DateTime}, {nameof(value.Battery)}:{value.Battery}, {nameof(value.ActiveAlarmBitmaps)}:{value.ActiveAlarmBitmaps}, {nameof(value.Glucose)}:{value.Glucose}";
                _logger.LogInformation(message);
                return value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception In ReadAlertAsync2: " + e.Message);
                _logger.LogCritical(e, e.Message);
                return default;
            }
        }

        #endregion  

        #region Time sync
        public async Task WriteTimeSynchronizationRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            try
            {
                if (device != null)
                    _device = device;

                if (_device == null)
                {
                    Debug.WriteLine("Request In WriteTimeSynchronizationRequestAsync: _device was null");
                    return;
                }

                byte[] request = new byte[5];

                byte[] oldTime = SugarBeatCharacteristicService.GetDateTimeBytes(DateTime.Now);

                Array.Copy(oldTime, request, oldTime.Length);

                request[4] = SugarBeatCharacteristicService.GetOneByteCrc(request);

                bool result = await _device?.Characteristics[CharacteristicType.TimeSynchronization]
                     .WriteAsync(request);

                if (result)
                {
                    Debug.WriteLine("Result of In WriteTimeSynchronizationRequestAsync: Passed");
                }
                else
                {
                    Debug.WriteLine("Result of In WriteTimeSynchronizationRequestAsync: failed retrying");
                    await Task.Delay(5000);
                    result = await _device?.Characteristics[CharacteristicType.TimeSynchronization]
                   .WriteAsync(request);
                }

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In WriteTimeSynchronizationRequestAsync: " + e.Message);
                return;
            }
        }

        #endregion

        #region History 

        public async Task<Object> WriteHistorytGlucoseRequestAsync(SugarBeatDevice device, CancellationToken cancellationToken, ushort index = 0, bool concurrentRequest = false)
        {
            try
            {
                if (device != null)
                    _device = device;

                IsHistorySyncDone = false;

                if (_device == null)
                {
                    Debug.WriteLine("Request In WriteHistorytGlucoseRequestAsync: _device was null");
                    return false;
                }

                byte[] request = new byte[7];

                if (!lastSyncTime.HasValue)
                {
                    //Get last sync time from DB
                    lastSyncTime = await getLastSyncTime();
                    //if (lastSyncTime.HasValue)
                    //{
                    //    lastSyncTime = lastSyncTime.Value.ToLocalTime();
                    //}
                }

                byte[] oldTime = SugarBeatCharacteristicService.GetDateTimeBytes(lastSyncTime.Value);

                Array.Copy(oldTime, request, oldTime.Length);

                var indexbytes = BitConverter.GetBytes(index);
                request[4] = indexbytes[0];
                request[5] = indexbytes[1];

                request[6] = SugarBeatCharacteristicService.GetOneByteCrc(request);

                string bitString = BitConverter.ToString(request);

                _logger.LogDebug("In WriteHistorytGlucoseRequestAsync requesting timestamp: " + lastSyncTime.Value + ", index: " + index);
                Debug.WriteLine("Payload In WriteHistorytGlucoseRequestAsync sync time : " + lastSyncTime.Value + ",  request length " + request.Length + " Value: " + bitString);
                bool result = false;

                result = await _device?.Characteristics[CharacteristicType.BulkGlucoseHistoryRequest]
               .WriteAsync(request);
                Debug.WriteLine("Payload In WriteHistorytGlucoseRequestAsync:request Ack: " + result);
                return result;
            }
            catch (DeviceConnectionException ex)
            {
                // Devce disconnected retrying the connection
                _logger.LogCritical(ex, ex.Message);
                Debug.WriteLine("DeviceConnectionException In WriteHistorytGlucoseRequestAsync: " + ex.Message);
                _eventAggregator.GetEvent<HistorySyncExceptionEvent>().Publish();

                return false;
            }
            catch (Exception e)
            {
                // Publishing this so that creating session can be carried out but sync is failed at this point
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In WriteHistorytGlucoseRequestAsync: " + e.Message);
                //   _eventAggregator.GetEvent<HistorySyncStatusEvent>().Publish(false);
                _eventAggregator.GetEvent<HistorySyncExceptionEvent>().Publish();
                return default;
            }
        }

        private async void HistoryResponseHeaderValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Payload In HistoryResponseHeaderValueUpdated: " + BitConverter.ToString(e.Characteristic.Value) + " " + e.Characteristic.Properties + " " + e.Characteristic.Value.Length);
                if (e.Characteristic.Value.Length == 11 && e.Characteristic.Properties == (CharacteristicPropertyType.Notify | CharacteristicPropertyType.Read))
                {
                    var responseheader = new BulkGlucoseHistoryResponseHeader(e.Characteristic.Value);

                    _logger.LogDebug("HistoryResponseHeaderValueUpdated - timestamp: " + responseheader.DateTime + ", count: " + responseheader.ReadCount);

                    var ctsource = new CancellationTokenSource();

                    if (responseheader.ReadCount > 0)
                    {
                        await Task.Delay(500);
                        readCount = (ushort)responseheader.ReadCount;
                        await ReadHistoryResponseBodyValueAsync(ctsource.Token);
                    }
                    else
                    {
                        if (lastSyncTime < DateTime.Today)
                        {
                            _logger.LogDebug("HistoryResponseHeaderValueUpdated - no items to fetch for timestamp " + responseheader.DateTime + ", skipping to next day.");

                            var dto = await _settingsService.GetByAccountIdAsync(ctsource.Token);
                            dto.Value.LastSyncedTime = lastSyncTime;
                            await _settingsService.UpdateAsync(dto.Value, ctsource.Token);

                            var day = lastSyncTime.Value.AddDays(1);
                            lastSyncTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);

                            await WriteHistorytGlucoseRequestAsync(_device, ctsource.Token, 0);
                        }
                        else
                        {
                            _logger.LogDebug("HistoryResponseHeaderValueUpdated - no items to fetch for timestamp " + responseheader.DateTime + ", no more history to download.");

                            //Stop reading history and update Sync time as date.today
                            var dto = await _settingsService.GetByAccountIdAsync(ctsource.Token);
                            dto.Value.LastSyncedTime = DateTime.Now;

                            await _settingsService.UpdateAsync(dto.Value, ctsource.Token);
                            IsHistorySyncDone = true;
                            _eventAggregator.GetEvent<HistorySyncStatusEvent>().Publish(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                Debug.WriteLine("Exception In HistoryResponseHeaderValueUpdated: " + ex.Message);
                _eventAggregator.GetEvent<HistorySyncExceptionEvent>().Publish();
            }
        }

        public async Task<bool> ReadHistoryResponseBodyValueAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_device == null)
                    return default;

                var readBytes = await _device.Characteristics[CharacteristicType.BulkGlucoseHistoryResponseBody]
                    .ReadAsync(cancellationToken);

                Debug.WriteLine("Payload In ReadHistoryResponseBodyValueAsync: " + BitConverter.ToString(readBytes));

                var value = new BulkGlucoseHistoryResponseBody(readBytes);

                var message = $"ReadHistoryResponseBodyValueAsync payload -> {nameof(value.DateTime)}:{value.DateTime}, " +
                    $"{nameof(value.Index)}:{value.Index}, " +
                    $"{nameof(value.Battery)}:{value.Battery}, " +
                    $"{nameof(value.ActiveAlarmBitmaps)}:{value.ActiveAlarmBitmaps}, " +
                    $"{nameof(value.Glucose)}:{value.Glucose}";
                _logger.LogInformation(message);
                Debug.WriteLine("Payload In ReadHistoryResponseBodyValueAsync: " + message);

                if (value.Glucose > 0)
                {
                    //This is glucose reading
                    var glucosevalue = new SugarBeatGlucose(value.DateTime, value.Battery, value.ActiveAlarmBitmaps, value.Glucose, readBytes);
                    SaveGlucose(glucosevalue);
                }
                else
                {
                    //This is Alarm reading
                    var alert = new SugarBeatAlert(value.DateTime, value.Battery, value.ActiveAlarmBitmaps,
                        value.Glucose, readBytes);
                    SaveAlert(alert);
                }

                Debug.WriteLine(message);

                var cts = new CancellationTokenSource();
                if (value.Index >= readCount - 1)
                {
                    if (lastSyncTime < DateTime.Today)
                    {
                        var day = lastSyncTime.Value.AddDays(1);
                        lastSyncTime = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                        await WriteHistorytGlucoseRequestAsync(_device, cts.Token, 0);
                        Debug.WriteLine("In ReadHistoryResponseBodyValueAsync: lastSyncTime: " + lastSyncTime + " Current Index : " + value.Index + " Index:0 ");
                    }
                    else
                    {
                        //Stop reading history and update Sync time as date.today
                        var ctsource = new CancellationTokenSource();
                        var dto = await _settingsService.GetByAccountIdAsync(ctsource.Token);

                        //TODO Check utc required
                        dto.Value.LastSyncedTime = DateTime.Now.ToUniversalTime();

                        await _settingsService.UpdateAsync(dto.Value, ctsource.Token);
                        IsHistorySyncDone = true;
                        _eventAggregator.GetEvent<HistorySyncStatusEvent>().Publish(true);
                    }
                }
                else
                {
                    await WriteHistorytGlucoseRequestAsync(_device, cts.Token, (ushort)(value.Index + 1));
                    Debug.WriteLine("In ReadHistoryResponseBodyValueAsync: lastSyncTime: " + lastSyncTime + " Current Index : " + value.Index + " Index: " + (value.Index + 1));
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception In ReadHistoryResponseBodyValueAsync: " + e.Message);
                _logger.LogCritical(e, e.Message);
                IsHistorySyncDone = false;
                // _eventAggregator.GetEvent<HistorySyncStatusEvent>().Publish(false);
                _eventAggregator.GetEvent<HistorySyncExceptionEvent>().Publish();
                return false;
            }
        }

        #endregion

        #region Connection and scan

        private CancellationToken tryReConnectToDeviceAsyncTocken;

        public async void TryReConnectToDeviceAsync(CancellationToken tocken)
        {
            tryReConnectToDeviceAsyncTocken = tocken;
            try
            {
                Debug.WriteLine("TryReConnectToDeviceAsync called");
                var model = await _secureStorageService.LoadObject<SugarBeatAccessDetails>(SecureStorageKey.SugarBeatAccessDetails);

                sugarBeatAccessDetails = await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = model.MacAddress,
                    Password = model.Password
                });
            }
            catch
            {
                sugarBeatAccessDetails = await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = string.Empty,
                    Password = string.Empty
                });
            }
            if (sugarBeatAccessDetails != null)
            {
                if (!String.IsNullOrEmpty(sugarBeatAccessDetails.Address) && !String.IsNullOrEmpty(sugarBeatAccessDetails.Password))
                {
                    Debug.WriteLine("TryReConnectToDeviceAsync calling StarScanAsync.");
                    _deviceName = sugarBeatAccessDetails.Address;
                    StarScanAsync(tocken, null, _deviceName);
                }
                else
                {
                    Debug.WriteLine("TryReConnectToDeviceAsync no acess details in local storage.");
                    _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                }
            }
            else
            {
                Debug.WriteLine("TryReConnectToDeviceAsync no acess details in local storage.");
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
            }
        }

        public async Task<BleScanResult> StarScanAsync(CancellationToken cancellationToken, IList<string> partialAddress, string name, int scanTimeout = 30000)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new BleScanResult(new Exception("Task Cancelled by user."));
                }

                if (_ble != null)
                {
                    if (_ble.Adapter != null)
                    {
                        if (_ble.Adapter.IsScanning)
                        {
                            return null;
                        }
                        else
                        {
                            if (_device != null)
                            {
                                var devs = _ble.Adapter.ConnectedDevices;
                                if (devs.Count > 0)
                                {
                                    if (devs.Contains(_device.PluginBleDevice))
                                    {
                                        Debug.WriteLine("Device is already connected");
                                        return new BleScanResult(_device);
                                    }
                                }
                            }
                        }
                    }
                }
                var res = await Init();
                if (res.Success)
                {
                    Debug.WriteLine("StarScanAsync called with device name " + name);

                    _device = null;
                    _deviceName = string.Empty;

                    if (partialAddress != null)
                    {
                        for (var i = 0; i < 4; i++)
                        {
                            _deviceName += partialAddress.ElementAt(i);
                        }
                    }
                    else
                    {
                        _deviceName = name;
                    }

                    _ble.Adapter.DeviceDiscovered -= AdapterDeviceDiscovered;
                    _ble.Adapter.ScanTimeoutElapsed -= Adapter_ScanTimeoutElapsed1;

                    _ble.Adapter.DeviceDiscovered += AdapterDeviceDiscovered;
                    _ble.Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed1;

                    _ble.Adapter.ScanTimeout = scanTimeout;
                    var guids = new Guid[] { SugarBeatDevice.ServiceId };

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new BleScanResult(new Exception("Task Cancelled by user."));
                    }
                    await _ble.Adapter.StartScanningForDevicesAsync(serviceUuids: guids, cancellationToken: cancellationToken);
                    return new BleScanResult(_device);
                }
                else
                {
                    _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                    return new BleScanResult(new Exception("BLE not initialized."));
                }
            }
            catch (Exception e)
            {
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In StarScanAsync: " + e.Message);
                return new BleScanResult(e);
            }
        }

        private void Adapter_ScanTimeoutElapsed1(object sender, EventArgs e)
        {
            try
            {
                if (_ble != null)
                {
                    Debug.WriteLine("Adapter_ScanTimeoutElapsed Stopping scan");

                    _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);

                    _ble.Adapter.ScanTimeoutElapsed -= Adapter_ScanTimeoutElapsed;
                    _ble.Adapter.DeviceDiscovered -= AdapterDeviceDiscovered;
                    StopScanAsync();
                    _eventAggregator.GetEvent<ScanForAvailableDevicesCompleted>().Publish();
                }

                //Scan completed event notify the caller that scan completed
                _eventAggregator.GetEvent<ScanForAvailableDevicesCompleted>().Publish();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " StackTrace: " + ex.StackTrace);
                Debug.WriteLine("Exception In Adapter_ScanTimeoutElapsed: " + ex.Message);
            }
        }

        public async Task ScanForAvailableDevicesAsync(CancellationToken cancellationToken,
          int scanTimeout = 10000)
        {
            try
            {
                if (_ble == null)
                {
                    var result = await Init();
                }

                //else
                //{
                //    if (_device != null)
                //    {
                //        var devs = _ble.Adapter.ConnectedDevices;
                //        if (devs.Count > 0)
                //        {
                //            if (devs.Contains(_device.PluginBleDevice))
                //            {
                //                Debug.WriteLine("Device is already connected");
                //                return new BleScanResult(_device);
                //            }
                //        }
                //    }
                //}


                if (_ble.Adapter.IsScanning)
                {
                    await StopScanAsync();
                }

                _device = null;

                _deviceName = string.Empty;

                _ble.Adapter.ScanTimeoutElapsed -= Adapter_ScanTimeoutElapsed;
                _ble.Adapter.DeviceDiscovered -= AdapterAvailableDeviceDiscovered;

                _ble.Adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
                _ble.Adapter.DeviceDiscovered += AdapterAvailableDeviceDiscovered;
                // _ble.Adapter.
                _ble.Adapter.ScanTimeout = scanTimeout;

                //To Find only sugarbeat devices
                var guids = new Guid[] { SugarBeatDevice.ServiceId };

                _ble.Adapter.StartScanningForDevicesAsync(serviceUuids: guids, cancellationToken: cancellationToken);

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In ScanForAvailableDevicesAsync: " + e.Message);
            }
        }

        private void AdapterAvailableDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (e?.Device != null)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    var cls = new CancellationTokenSource().Token;                    
                    var x = e.Device.AdvertisementRecords;
                    Debug.WriteLine("RSSI : " + e.Device.Rssi);
                    Debug.WriteLine("Advertizement Records");
                    foreach (var rec in e.Device.AdvertisementRecords)
                    {
                        Debug.WriteLine(rec.Type + " " + rec.Data);
                    }
                    var service = e.Device.GetServiceAsync(SugarBeatDevice.ServiceId, cls);

                    if (service != null)
                    {
                        PropertyInfo propInfo = e?.Device?.NativeDevice?.GetType()?.GetProperty("Address");
                        string address = (string)propInfo?.GetValue(e?.Device.NativeDevice, null);
                        SugarBeatDevice discoveredDevice;
                        Debug.WriteLine("DeviceDiscovered: name " + e.Device.Name + " Id: " + e.Device.Id);
                        if (String.IsNullOrEmpty(address))
                        {
                            discoveredDevice = new SugarBeatDevice(e.Device.Id, e.Device.Name, e.Device.Rssi) { PluginBleDevice = e.Device };
                        }
                        else
                        {
                            discoveredDevice = new SugarBeatDevice(e.Device.Id, e.Device.Name, address, e.Device.Rssi) { PluginBleDevice = e.Device };
                        }
                        _eventAggregator.GetEvent<BlueTooothDeviceDiscoveredEvent>().Publish(discoveredDevice);
                    }
                    else
                    {
                        Debug.WriteLine("AdapterAvailableDeviceDiscovered->Service null in DeviceDiscovered: name " + e.Device.Name + " Id: " + e.Device.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + " StackTrace: " + ex.StackTrace);
                    Debug.WriteLine("Exception In AdapterAvailableDeviceDiscovered:  " + ex.Message);
                }
            }
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            try
            {
                if (_ble != null)
                {
                    Debug.WriteLine("Adapter_ScanTimeoutElapsed Stopping scan");
                    _ble.Adapter.DeviceDiscovered -= AdapterAvailableDeviceDiscovered;

                    StopScanAsync();
                }

                //Scan completed event notify the caller that scan completed
                _eventAggregator.GetEvent<ScanForAvailableDevicesCompleted>().Publish();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " StackTrace: " + ex.StackTrace);
                Debug.WriteLine("Exception In Adapter_ScanTimeoutElapsed: " + ex.Message);
            }
        }

        public bool CheckSugarbeatConnected()
        {
            return IsConnected;
        }

        public async Task<bool> ConnectDeviceAsync(SugarBeatDevice device, CancellationToken cancellationToken, bool autoconnect = false)
        {
            try
            {
                tryReConnectToDeviceAsyncTocken = cancellationToken;
                if (device == null)
                {
                    return false;
                }

                if (!device.DeviceFound)
                {
                    return false;
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Task cancelled in ConnectDeviceAsync -1");
                    return false;
                }

                await _ble.Adapter.ConnectToDeviceAsync(device.PluginBleDevice,
                   new ConnectParameters(autoConnect: autoconnect, forceBleTransport: true), cancellationToken);

                switch (device.PluginBleDevice.State)
                {
                    case DeviceState.Disconnected:
                        if (cancellationToken.IsCancellationRequested)
                        {
                            Debug.WriteLine("Task cancelled in ConnectDeviceAsync- Disconnected -2");
                            return false;
                        }
                        Debug.WriteLine("Calling ConnectDeviceAsync for Transmitter Reconnect as DeviceState.Disconnected");
                        return await ConnectDeviceAsync(device, cancellationToken);
                    case DeviceState.Limited:
                    case DeviceState.Connecting:
                        return false;

                    case DeviceState.Connected:
                        Debug.WriteLine("Connected to device");
                        _device = device;
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (DeviceConnectionException deviceConnectionException)
            {
                deviceExceptionRetryCount++;
                _logger.LogError(deviceConnectionException, deviceConnectionException.Message);

                Debug.WriteLine("Exception In ConnectDeviceAsync: DeviceConnectionException :" + deviceConnectionException.Message);
                Debug.WriteLine("Transmitter connection failed. trying to reconnect");
                Debug.WriteLine("Calling ConnectDeviceAsync for Transmitter Reconnect.");
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Task cancelled in ConnectDeviceAsync- DeviceConnectionException");
                    deviceExceptionRetryCount = 0;
                    return false;
                }

                if (deviceExceptionRetryCount < 4)
                {
                    Debug.WriteLine(" Calling ConnectDeviceAsync " + deviceExceptionRetryCount);
                    await Task.Delay(500);
                    return await ConnectDeviceAsync(device, cancellationToken);
                }
                else
                {
                    deviceExceptionRetryCount = 0;
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                Debug.WriteLine("Exception In ConnectDeviceAsync: " + e.Message);
                _device = null;
                return false;
            }
        }

        int deviceExceptionRetryCount = 0;

        private bool IsDisconnectedManually = false;

        public async Task DisconnectDeviceAsync(SugarBeatDevice device = null, bool isDisconnectedManually = false)
        {
            Debug.WriteLine("DisconnectDeviceAsync called is Disconnected manually." + isDisconnectedManually);
            IsDisconnectedManually = isDisconnectedManually;
            if (_ble != null)
            {
                try
                {
                    _ble.StateChanged -= OnStateChanged;
                    _ble.Adapter.DeviceConnected -= AdapterDeviceConnected;
                    _ble.Adapter.DeviceDisconnected -= AdapterDeviceDisconnected;
                    await StopScanAsync();

                    if (_device != null)
                    {
                        // Unsubscribing event handlers fix for hisory header being subscribed twice.
                        try
                        {
                            if (_device.Characteristics.ContainsKey(CharacteristicType.CurrentGlucose))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.CurrentGlucose];

                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= CurrentGlucoseValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Excepion in CurrentGlucose StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                            Debug.WriteLine("Excepion in CurrentGlucose StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                        }

                        try
                        {
                            if (_device.Characteristics.ContainsKey(CharacteristicType.BulkGlucoseHistoryResponseHeader))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.BulkGlucoseHistoryResponseHeader];

                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= HistoryResponseHeaderValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Excepion in BulkGlucoseHistoryResponseHeader StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                            Debug.WriteLine("Excepion in BulkGlucoseHistoryResponseHeader StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                        }

                        try
                        {
                            if (_device.Characteristics.ContainsKey(CharacteristicType.PasskeyAuthenticationResponse))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.PasskeyAuthenticationResponse];

                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= PassKeyValueResponse_ValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Excepion in PasskeyAuthenticationResponse StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                            Debug.WriteLine("Excepion in PasskeyAuthenticationResponse StopUpdatesAsync in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                        }

                        try
                        {
                            //This should unsubscribe from events 
                            _device.ClearCharacteristic();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Excepion in ClearCharacteristic in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                            Debug.WriteLine("Excepion in ClearCharacteristic in DisconnectDeviceAsync " + ex.Message + " Stacktrace:" + ex.StackTrace);
                        }

                        if (_device.PluginBleDevice.State == DeviceState.Connected)
                        {
                            Debug.WriteLine("Calling Adpter DisconnectDeviceAsync.");
                            await _ble.Adapter.DisconnectDeviceAsync(_device.PluginBleDevice);
                        }
                        _logger.LogDebug("Successful: Disconnect in DisconnectDeviceAsync. ");
                        Debug.WriteLine("Successful: Disconnect in DisconnectDeviceAsync.");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Exception: Disconnect in DisconnectDeviceAsync. ");
                    Debug.WriteLine("Exception In DisconnectDeviceAsync: " + e.Message + " Stacktrace:" + e.StackTrace);
                    _logger.LogError(e, e.Message);
                }
                finally
                {
                    _device = null;
                }
            }
        }


        private async void AdapterDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            try
            {
                _logger.LogDebug($"Event -> sugarBEAT: {e.Device.Name} disconnected");
                Debug.WriteLine($"Event -> sugarBEAT: {e.Device.Name} disconnected");

                if (IsHistorySyncDone)
                {
                    _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Publish(e.Device.Id);
                }
                Debug.WriteLine($"Event -> sugarBEAT: {e.Device.Name} disconnected calling ConnectDeviceAsync again for the same device.");
                if (!IsDisconnectedManually)
                {
                    Debug.WriteLine("Divice was disconnected automatically and trying to reconnect");

                    //if (IsHistorySyncDone)
                    //{
                    //    //Doing the complete init process and char enrollment 
                    //    _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko Device Disconected. Trying to reconnect.");
                    //}
                    // IsConnected = false;
                    Debug.WriteLine("Stopping sync.");
                    await CleanUpAsync();

                    await Task.Delay(500);
                    if (await ConnectDeviceAsync(_device, tryReConnectToDeviceAsyncTocken))
                    {

                        // var cts = new CancellationTokenSource().Token;
                        Debug.WriteLine("Calling InitAllCharacteristicAsyncsync for Device Reconnect.");
                        var result = await InitAllCharacteristicAsync(_device, tryReConnectToDeviceAsyncTocken);
                        if (result == true)
                        {
                            Debug.WriteLine("Calling InitAllCharacteristicAsyncsync for Device Reconnect.");

                            var authresult = await WritePasskeyAuthenticationAsync(_device, sugarBeatAccessDetails.Password, tryReConnectToDeviceAsyncTocken);
                            if (authresult)
                            {
                                //Wait for the pass key response
                            }
                            else
                            {
                                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                                Debug.WriteLine(" Failed while WritePasskeyAuthenticationAsync  in AdapterDeviceDisconnected");
                            }
                        }
                        else
                        {
                            _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                            Debug.WriteLine("Failed while InitAllCharacteristicAsync in AdapterDeviceDisconnected ");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("reconnect failed in AdapterDeviceDisconnected ");
                        _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AdapterDeviceDisconnected => exception: " + ex.Message);
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
            }
        }

        private void AdapterDeviceConnected(object sender, DeviceEventArgs e)
        {
            try
            {
                _logger.LogDebug($"Event -> sugarBEAT: {e.Device.Name} connected");
                Debug.WriteLine($"Event -> sugarBEAT: {e.Device.Name} connected");
                //  IsConnected = true;
                IsDevicePaired = true;

                _eventAggregator.GetEvent<BleDeviceConnectedEvent>().Publish(e.Device.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AdapterDeviceConnected => exception: " + ex.Message);
            }
        }

        private CancellationTokenSource tocken;

        private async void AdapterDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            try
            {
                if (tryReConnectToDeviceAsyncTocken != null)
                {
                    if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Task cancelled in AdapterDeviceDiscovered -1");
                        await StopScanAsync();
                        return;
                    }
                }

                if (e.Device != null)
                {
                    _logger.LogDebug($"Device discovered -> Id: {e.Device.Id}, Name: {e.Device.Name}");
                    Debug.WriteLine($"Device discovered -> Id: {e.Device.Id}, Name: {e.Device.Name}");

                    if (e.Device.Name?.ToLower() == _deviceName)
                    {
                        _device = new SugarBeatDevice(e.Device.Id, e.Device.Name, e.Device.Rssi) { PluginBleDevice = e.Device };

                        _ble.Adapter.DeviceDiscovered -= AdapterDeviceDiscovered;
                        _ble.Adapter.ScanTimeoutElapsed -= Adapter_ScanTimeoutElapsed1;
                        Debug.WriteLine("Stopping sync.");
                        await StopScanAsync();

                        var count = 0;

                        if (tryReConnectToDeviceAsyncTocken != null)
                        {
                            if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                            {
                                Debug.WriteLine("Task cancelled in AdapterDeviceDiscovered -2");
                                return;
                            }
                        }
                        do
                        {
                            Debug.WriteLine("Retrying connection on connection failed attempt :  " + count++);
                            if (tryReConnectToDeviceAsyncTocken != null)
                            {
                                if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                                {
                                    Debug.WriteLine("Task cancelled in AdapterDeviceDiscovered -5");
                                    return;
                                }
                            }
                            var connResult = await ConnectDeviceAsync(_device, tryReConnectToDeviceAsyncTocken);
                            Debug.WriteLine("Retrying connection on connection failed attempt result :  " + connResult);
                            if (connResult == true)
                            {
                                Debug.WriteLine("Calling InitAllCharacteristicAsyncsync for Device Reconnect.");
                                //Initiallizing characteristics
                                if (tryReConnectToDeviceAsyncTocken != null)
                                {
                                    if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                                    {
                                        Debug.WriteLine("Task cancelled in AdapterDeviceDiscovered -3 calling clean up.");
                                        return;
                                    }
                                }

                                var result = await InitAllCharacteristicAsync(_device, tryReConnectToDeviceAsyncTocken);
                                if (result == true)
                                {
                                    Debug.WriteLine("Calling InitAllCharacteristicAsyncsync for Device Reconnect.");

                                    if (tryReConnectToDeviceAsyncTocken != null)
                                    {
                                        if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                                        {
                                            Debug.WriteLine("Task cancelled in AdapterDeviceDiscovered -4");
                                            return;
                                        }
                                    }
                                    //Writing pass key
                                    var authresult = await WritePasskeyAuthenticationAsync(_device, sugarBeatAccessDetails.Password, tryReConnectToDeviceAsyncTocken);
                                    if (authresult)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        //Connection failed intimate user that passkey authentication failed try again. Take the user back to scan page.


                                        if (tryReConnectToDeviceAsyncTocken != null)
                                        {
                                            if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                                            {
                                                Debug.WriteLine("WritePasskeyAuthenticationAsync returned false and task cancelled so calling clean up ");

                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (tryReConnectToDeviceAsyncTocken != null)
                                    {
                                        if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                                        {
                                            Debug.WriteLine("InitAllCharacteristicAsync returned false and task cancelled so calling clean up ");

                                            return;
                                        }
                                    }
                                    //Connection failed                               
                                    Debug.WriteLine("InitAllCharacteristicAsync returned false");
                                }
                            }
                            count++;
                            await Task.Delay(500);

                            Debug.WriteLine("In AdapterDeviceDiscovered -> Retry count " + count);
                        } while (count < 5);

                        //Connection failed
                        _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
                        Debug.WriteLine("Reconnect failed while ConnectDeviceAsync ");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Reconnect failed: " + ex.Message);
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(false);
            }
        }

        public async Task StopScanAsync()
        {
            try
            {
                if (_ble == null)
                {
                    return;
                }

                if (_ble.Adapter.IsScanning)
                {
                    await _ble.Adapter.StopScanningForDevicesAsync();
                    _logger.LogDebug("Successful: stop scanning.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                Debug.WriteLine("Exception In StopScanAsync: " + e.Message);
            }
        }

        private async void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            try
            {
                _logger.LogDebug($"BluetoothState: {e.NewState}");
                Debug.WriteLine($"BluetoothState: {e.NewState}");

                switch (e.NewState)
                {
                    case BluetoothState.On:
                        if (IsBluetoothOn)
                        {
                            // Verhindern, dass der Event mehrere Male gefeuert wird.
                            //Prevent the event from being fired multiple times.
                            return;
                        }
                        IsBluetoothOn = true;
                        if (tryReConnectToDeviceAsyncTocken == null)
                        {
                            tryReConnectToDeviceAsyncTocken = new CancellationTokenSource().Token;
                        }
                        TryReConnectToDeviceAsync(tryReConnectToDeviceAsyncTocken);
                        break;

                    case BluetoothState.Unknown:
                    case BluetoothState.Unavailable:
                    case BluetoothState.Unauthorized:
                    case BluetoothState.TurningOn:
                    case BluetoothState.TurningOff:
                    case BluetoothState.Off:
                        if (!IsBluetoothOn)
                        {
                            // Verhindern, dass der Event mehrere Male gefeuert wird.
                            //Prevent the event from being fired multiple times.
                            return;
                        }
                        IsBluetoothOn = false;
                        await CleanUpAsync(_device);
                        _eventAggregator.GetEvent<BluetoothStateEvent>().Publish(false);
                        _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Blutooth is turned off. Please turn it on.");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnStateChanged => exception: " + ex.Message);
            }
        }

        private async void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            try
            {
                if (e.Device != null)
                {
                    Debug.WriteLine("Adapter_DeviceConnectionLost : " + e.Device.State);
                    if (e.Device.State != DeviceState.Connected)
                    {
                        Debug.WriteLine("Adapter_DeviceConnectionLost calling CleanUpAsync");
                        await CleanUpAsync(_device);
                        _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko device is disconnected or out of range. Please connect again.");
                    }
                }
                else
                {
                    Debug.WriteLine("Adapter_DeviceConnectionLost Device was null  calling CleanUpAsync");
                    await CleanUpAsync(_device);
                    _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko device is disconnected or out of range. Please connect again.");
                }

                Debug.WriteLine("Device lost recieved in ble service");

                _eventAggregator.GetEvent<BleDeviceDisconnectedStatusEvent>().Publish();
                Debug.WriteLine("Starting TryReconnectAfterDeviceLost in Adapter_DeviceConnectionLost");

                TryReconnectAfterDeviceLost();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Adapter_DeviceConnectionLost => exception: " + ex.Message);
            }
        }

        private void TryReconnectAfterDeviceLost()
        {
            try
            {
                Debug.WriteLine("Starting retries to reconnect in TryReconnectAfterDeviceLost");
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        var time = DateTime.Now;
                        var counter = 50;
                        do
                        {
                            if (counter > 45)
                            {
                                await Task.Delay(Convert.ToInt32(1000 * 60 * .5));
                            }
                            if (counter <= 45 && counter > 0)
                            {
                                await Task.Delay(1000 * 60 * 2);
                            }
                            counter--;

                            if (IsConnected)
                            {
                                break;
                            }
                            Debug.WriteLine("Calling TryReConnectToDeviceAsync in TryReconnectAfterDeviceLost retry count:" + counter);

                            tryReConnectToDeviceAsyncTocken = new CancellationTokenSource().Token;

                            if (!tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                            {
                                TryReConnectToDeviceAsync(tryReConnectToDeviceAsyncTocken);
                            }
                            else
                            {
                                Debug.WriteLine("tryReConnectToDeviceAsyncTocken IsCancellationRequested true in TryReconnectAfterDeviceLost");
                            }
                        } while (counter >= 0);

                        Debug.WriteLine("Exiting Reconnection retries after Device lost IsConnected " + IsConnected);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("TryReconnectAfterDeviceLost => exception: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TryReconnectAfterDeviceLost => exception: " + ex.Message);
            }
        }

        public async Task CleanUpAsync(SugarBeatDevice device = null, bool IsDisconnectedManually = false)
        {
            if (_ble != null)
            {
                try
                {
                    await StopScanAsync();

                    if (_device != null)
                    {
                        try
                        {
                            if (_device.Characteristics.Keys.Contains(CharacteristicType.PasskeyAuthenticationResponse))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.PasskeyAuthenticationResponse];
                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= PassKeyValueResponse_ValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            Debug.WriteLine("Exception In CleanUpAsync PasskeyAuthenticationResponse: " + ex.Message);
                        }

                        try
                        {
                            if (_device.Characteristics.Keys.Contains(CharacteristicType.CurrentGlucose))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.CurrentGlucose];
                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= CurrentGlucoseValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            Debug.WriteLine("Exception In CleanUpAsync CurrentGlucose: " + ex.Message);
                        }

                        try
                        {
                            if (_device.Characteristics.Keys.Contains(CharacteristicType.Alert))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.Alert];
                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= AlertValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            Debug.WriteLine("Exception In CleanUpAsync Alert: " + ex.Message);
                        }

                        try
                        {
                            if (_device.Characteristics.Keys.Contains(CharacteristicType.BulkGlucoseHistoryResponseHeader))
                            {
                                var characteristic = _device.Characteristics[CharacteristicType.BulkGlucoseHistoryResponseHeader];
                                if (characteristic != null)
                                {
                                    characteristic.ValueUpdated -= HistoryResponseHeaderValueUpdated;
                                    await characteristic.StopUpdatesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                            Debug.WriteLine("Exception In CleanUpAsync BulkGlucoseHistoryResponseHeader: " + ex.Message);
                        }

                        Debug.WriteLine("Calling Disconnect from CleanupAsync.");
                        _device.ClearCharacteristic();
                        await DisconnectDeviceAsync(_device, IsDisconnectedManually);
                    }

                    _logger.LogDebug("Successful: Clean up.");
                    Debug.WriteLine("Successful: Clean up.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    Debug.WriteLine("Exception In CleanUpAsync: " + e.Message);
                    _device = null;
                }
            }
        }
        #endregion

        #region PassKey authentication

        public async Task<bool> WritePasskeyAuthenticationAsync(SugarBeatDevice device, string passkeyAuthentication,
            CancellationToken cancellationToken)
        {
            try
            {
                sugarBeatAccessDetails.Address = device.MacAddress;
                if (tryReConnectToDeviceAsyncTocken != null)
                {
                    if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Task cancelled in WritePasskeyAuthenticationAsync -1");
                        return false;
                    }
                }
                //  SugarBeatDetailsProxy
                return await device.Characteristics[CharacteristicType.PasskeyAuthenticationRequest]
                    .WriteAsync(SugarBeatCharacteristicService.GetPasskeyPayload(passkeyAuthentication),
                        cancellationToken);
            }
            catch (DeviceConnectionException ex)
            {
                // Devce disconnected retrying the connection
                _logger.LogCritical(ex, ex.Message);
                Debug.WriteLine("Exception In WritePasskeyAuthenticationAsync: " + ex.Message);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In WritePasskeyAuthenticationAsync: " + e.Message);
                return false;
            }
        }

        public async Task<bool> ReadPasskeyAuthenticationResponseAsync(SugarBeatDevice device,
            CancellationToken cancellationToken)
        {
            try
            {
                var readBytes = await device.Characteristics[CharacteristicType.PasskeyAuthenticationResponse]
                    .ReadAsync(cancellationToken);
                var bytesForCrc = new byte[1];
                Array.Copy(readBytes, bytesForCrc, 1);
                if (SugarBeatCharacteristicService.GetOneByteCrc(bytesForCrc) == readBytes.ElementAt(1))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception In ReadPasskeyAuthenticationResponseAsync: " + e.Message);
                _logger.LogCritical(e, e.Message);
                return false;
            }
        }

        private async void PassKeyValueResponse_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Payload In PassKeyValueResponse_ValueUpdated: " + e.Characteristic.Value);
                string bitString = BitConverter.ToString(e.Characteristic.Value);
                Debug.WriteLine("Payload In PassKeyValueResponse_ValueUpdated:value length " + e.Characteristic.Value.Length + " Value: " + bitString);

                if (e.Characteristic.Value.Length == 2 && e.Characteristic.Properties == (Plugin.BLE.Abstractions.CharacteristicPropertyType.Notify | Plugin.BLE.Abstractions.CharacteristicPropertyType.Read))
                {
                    Debug.WriteLine("Payload In PassKeyValueResponse_ValueUpdated: " + BitConverter.ToString(e.Characteristic.Value));

                    if (e.Characteristic.Value.Length == 2)
                    {
                        try
                        {
                            var value = new SugarBeatPasskeyAuthResponse(e.Characteristic.Value);
                            var message = $"PassKeyValueResponse_ValueUpdated payload -> {nameof(value.authenticationStatus)}:{value.authenticationStatus}";
                            _logger.LogInformation(message);

                            if (value.authenticationStatus == true)
                            {

                                var model = new SugarBeatAccessDetails { MacAddress = _deviceName, Password = sugarBeatAccessDetails.Password };
                                await _secureStorageService.SaveObject(SecureStorageKey.SugarBeatAccessDetails, model);

                                var cancellationToken = new CancellationTokenSource().Token;
                                Debug.WriteLine("Calling WriteTimeSynchronizationRequestAsync for Device Reconnect after pass key authentiated sucessfully.");
                                await WriteTimeSynchronizationRequestAsync(_device, cancellationToken);
                                var tocken = new CancellationTokenSource().Token;
                                Debug.WriteLine("Calling WriteHistorytGlucoseRequestAsync for Device Reconnect.");
                                Debug.WriteLine("Reconnect successfull");
                                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Publish(true);
                                // Delay to workaround transmitter bond saving issue if history request happens too quickly after connect
                                await Task.Delay(2000);
                                _eventAggregator.GetEvent<HistorySyncStartedEvent>().Publish(true);

                                _eventAggregator?.GetEvent<HistorySyncExceptionEvent>().Unsubscribe(OnHistorySyncException);
                                _eventAggregator?.GetEvent<HistorySyncExceptionEvent>().Subscribe(OnHistorySyncException);

                                await WriteHistorytGlucoseRequestAsync(_device, tocken, 0);
                            }
                            else
                            {
                                // PassKey Failed scrnario removing cred from local storage as pin entered was incorrect.
                                await _secureStorageService.Remove(SecureStorageKey.SugarBeatAccessDetails);
                                // PassKey Failed scrnario to handle Disconnect client
                                Debug.WriteLine("DisconnectDeviceAsync in PassKeyValueResponse_ValueUpdated called is Disconnected manually false.authenticationStatus :failed");
                                await DisconnectDeviceAsync(_device);
                                //TODO Notify Device couldnot be connected
                                _eventAggregator.GetEvent<PassKeyAuthFailedEvent>().Publish();
                            }
                        }

                        catch (Exception exception)
                        {
                            _logger.LogCritical(exception, exception.Message);
                            Debug.WriteLine("Exception In PassKeyValueResponse_ValueUpdated: " + exception.Message);
                            //TODO PassKey Failed scrnario to handle Disconnect client

                            await DisconnectDeviceAsync(_device);
                            //TODO Notify Device couldnot be connected
                            _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Publish();
                        }
                    }
                    else
                    {
                        //TODO PassKey Failed scrnario to handle
                        _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko Device connection failed. Pin entered is invalid. Please try again.");

                        Debug.WriteLine("DisconnectDeviceAsync in PassKeyValueResponse_ValueUpdated called is Disconnected manually false e.Characteristic.Value.Length != 2 :" + e.Characteristic.Value.Length);
                        await DisconnectDeviceAsync(_device);
                        _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Publish();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PassKeyValueResponse_ValueUpdated => exception: " + ex.Message);
                _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Publish();
            }
        }

        private async void OnHistorySyncException()
        {
            try
            {
                await DisconnectDeviceAsync(_device, false);

                TryReConnectToDeviceAsync(tryReConnectToDeviceAsyncTocken);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Alert 

        private void AlertValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                IsSessionExpired();
                string bitString = BitConverter.ToString(e.Characteristic.Value);
                Debug.WriteLine("Payload In AlertValueUpdated:value length " + e.Characteristic.Value.Length + " Value: " + bitString);

                if (e.Characteristic.Value.Length == 17 && e.Characteristic.Properties == (Plugin.BLE.Abstractions.CharacteristicPropertyType.Notify | Plugin.BLE.Abstractions.CharacteristicPropertyType.Read))
                {
                    try
                    {
                        var alert = new SugarBeatAlert(e.Characteristic.Value);
                        _eventAggregator?.GetEvent<SugarBeatAlertEvent>().Publish(alert);
                        PublishAlert(alert);
                        var message = $"AlertValueUpdated payload -> {nameof(alert.DateTime)}:{alert.DateTime}, {nameof(alert.ActiveAlarmBitmap)}:{alert.ActiveAlarmBitmap}, {nameof(alert.CriticalAlarmCode)}:{alert.CriticalAlarmCode}";
                        _logger.LogInformation(message);
                        if (alert != null)
                        {
                            SaveAlert(alert);
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, exception.Message);
                        Debug.WriteLine("Exception In AlertValueUpdated: " + exception.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AlertValueUpdated => exception: " + ex.Message);
            }
        }

        public async Task<SugarBeatAlert> ReadAlertAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            try
            {
                var readBytes = await device.Characteristics[CharacteristicType.Alert]
                    .ReadAsync(cancellationToken);
                var alert = new SugarBeatAlert(readBytes);
                var message = $"Read Alert -> {nameof(alert.DateTime)}:{alert.DateTime}, {nameof(alert.ActiveAlarmBitmap)}:{alert.ActiveAlarmBitmap}, {nameof(alert.CriticalAlarmCode)}:{alert.CriticalAlarmCode}";
                _logger.LogInformation(message);
                return alert;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In ReadAlertAsync1: " + e.Message);
                return default;
            }
        }

        public async Task<SugarBeatAlert> ReadAlertAsync(CancellationToken cancellationToken)
        {
            try
            {
                var readBytes = await _device.Characteristics[CharacteristicType.Alert]
                    .ReadAsync(cancellationToken);
                var alert = new SugarBeatAlert(readBytes);
                var message = $"Read Alert -> {nameof(alert.DateTime)}:{alert.DateTime}, {nameof(alert.ActiveAlarmBitmap)}:{alert.ActiveAlarmBitmap}, {nameof(alert.CriticalAlarmCode)}:{alert.CriticalAlarmCode}";
                _logger.LogInformation(message);
                return alert;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In ReadAlertAsync2: " + e.Message);
                return default;
            }
        }
        #endregion

        #region Init

        public async Task<BleInitResult> Init()
        {
            try
            {
                if (_ble == null)
                {
                    _ble = CrossBluetoothLE.Current;
                    _ble.Adapter.ScanMode = ScanMode.LowLatency;
                    _ble.StateChanged -= OnStateChanged;
                    _ble.StateChanged += OnStateChanged;
                    _ble.Adapter.DeviceConnected -= AdapterDeviceConnected;
                    _ble.Adapter.DeviceConnected += AdapterDeviceConnected;
                    _ble.Adapter.DeviceDisconnected -= AdapterDeviceDisconnected;
                    _ble.Adapter.DeviceDisconnected += AdapterDeviceDisconnected;
                    _ble.Adapter.DeviceConnectionLost -= Adapter_DeviceConnectionLost;
                    _ble.Adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;
                }
                else
                {
                    await CleanUpAsync(_device);
                }

                IsBluetoothOn = _ble.State == BluetoothState.On;
                _logger.LogDebug("Successful: Init.");
                return await Task.FromResult(BleInitResult.Successful);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                Debug.WriteLine("Exception In Init: " + e.Message);
                return new BleInitResult(e);
            }
        }

        public async Task<bool> InitAllCharacteristicAsync(SugarBeatDevice device, CancellationToken cancellationToken)
        {
            try
            {
                if (device == null)
                {
                    Debug.WriteLine("Device null in InitAllCharacteristicAsync");
                    return false;
                }

                if (tryReConnectToDeviceAsyncTocken != null)
                {
                    if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Task cancelled in InitAllCharacteristicAsync -1");
                        return false;
                    }
                }
                var service =
                    await device.PluginBleDevice.GetServiceAsync(SugarBeatDevice.ServiceId, tryReConnectToDeviceAsyncTocken);


                if (service == null)
                {
                    _logger.LogDebug($"Service (GUID: {SugarBeatDevice.ServiceId}) not found");
                    //
                    return false;
                }

                // Passkey Authentication Request Characteristic
                if (!await AddCharacteristic(service, device, CharacteristicType.PasskeyAuthenticationRequest))
                {
                    return false;
                }
                // Passkey Authentication Response Characteristic
                if (!await AddCharacteristic(service, device, CharacteristicType.PasskeyAuthenticationResponse))
                {
                    return false;
                }
                // Alert Characteristic
                if (!await AddCharacteristic(service, device, CharacteristicType.Alert))
                {
                    return false;
                }
                //Current Glucose Characteristic
                if (!await AddCharacteristic(service, device, CharacteristicType.CurrentGlucose))
                {
                    return false;
                }
                if (!await AddCharacteristic(service, device, CharacteristicType.BulkGlucoseHistoryRequest))
                {
                    return false;
                }
                if (!await AddCharacteristic(service, device, CharacteristicType.BulkGlucoseHistoryResponseHeader))
                {
                    return false;
                }
                if (!await AddCharacteristic(service, device, CharacteristicType.BulkGlucoseHistoryResponseBody))
                {
                    return false;
                }
                if (!await AddCharacteristic(service, device, CharacteristicType.TimeSynchronization))
                {
                    return false;
                }

                if (tryReConnectToDeviceAsyncTocken != null)
                {
                    if (tryReConnectToDeviceAsyncTocken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Task cancelled in InitAllCharacteristicAsync -2");

                        return false;
                    }
                }
                var settings = await _settingsService.GetByAccountIdAsync(tryReConnectToDeviceAsyncTocken);
                if (settings != null)
                {
                    if (settings.Value == null)
                    {
                        //create new settings
                        Debug.WriteLine("Saving new Device settigs");

                        SugarBeatSettingDto newSettings = new SugarBeatSettingDto() { DeviceId = device.PluginBleDevice.Id, TransmitterId = device.Name };
                        _settingsService.AddAsync(newSettings, tryReConnectToDeviceAsyncTocken);
                    }
                    else
                    {
                        if (settings?.Value?.TransmitterId != device.Name)
                        {
                            Debug.WriteLine("Update Device settigs with connected device.");
                            settings.Value.DeviceId = device.PluginBleDevice.Id;
                            settings.Value.TransmitterId = device.Name;
                            _settingsService.UpdateAsync(settings.Value, tryReConnectToDeviceAsyncTocken);
                        }
                    }
                }
                Debug.WriteLine(" InitAllCharacteristicAsync success returning true");
                return true;
            }
            catch (DeviceConnectionException ex)
            {
                // Devce disconnected retrying the connection
                _logger.LogCritical(ex, ex.Message);
                Debug.WriteLine("Exception In InitAllCharacteristicAsync: " + ex.Message);

                //await Task.Delay(500);
                //TryReConnectToDeviceAsync(cancellationToken);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                Debug.WriteLine("Exception In InitAllCharacteristicAsync: " + e.Message);
                return false;
            }
        }

        private async Task<bool> AddCharacteristic(IService service, SugarBeatDevice device, CharacteristicType type)
        {
            try
            {
                if (device == null)
                {
                    Debug.WriteLine("Device null in AddCharacteristic");

                    return false;
                }
                if (service == null)
                {
                    Debug.WriteLine("Service null in AddCharacteristic");

                    return false;
                }

                Guid id;
                if (device.HasCharacteristic(type))
                {
                    _logger.LogDebug($"Successful: characteristics exist (GUID: {device.Characteristics[type].Id})");
                    Debug.WriteLine($"Successful: characteristics exist (GUID: {device.Characteristics[type].Id})");

                    return true;
                }

                switch (type)
                {
                    case CharacteristicType.PasskeyAuthenticationRequest:
                        id = SugarBeatDevice.PasskeyAuthenticationRequestCharacteristicId;
                        break;

                    case CharacteristicType.PasskeyAuthenticationResponse:
                        id = SugarBeatDevice.PasskeyAuthenticationResponseCharacteristicId;

                        break;

                    case CharacteristicType.Alert:
                        id = SugarBeatDevice.AlertCharacteristicId;
                        break;

                    case CharacteristicType.CurrentGlucose:
                        id = SugarBeatDevice.CurrentGlucoseCharacteristicId;
                        break;

                    case CharacteristicType.BulkGlucoseHistoryRequest:
                        id = SugarBeatDevice.HistoryRequestCharacteristicId;
                        break;

                    case CharacteristicType.BulkGlucoseHistoryResponseHeader:
                        id = SugarBeatDevice.HistoryResponseHeaderCharacteristicId;
                        break;

                    case CharacteristicType.BulkGlucoseHistoryResponseBody:
                        id = SugarBeatDevice.HistoryResponseBodyCharacteristicId;
                        break;

                    case CharacteristicType.TimeSynchronization:
                        id = SugarBeatDevice.SyncTimeCharacteristicsId;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                var characteristic = await service.GetCharacteristicAsync(id);
                if (characteristic == null)
                {
                    _logger.LogDebug($"Characteristic not found. (GUID: {characteristic.Id})");
                    Debug.WriteLine($"Characteristic not found. (GUID: {characteristic.Id})");
                    return false;
                }

                switch (type)
                {
                    case CharacteristicType.PasskeyAuthenticationRequest:
                        break;

                    case CharacteristicType.PasskeyAuthenticationResponse:
                        characteristic.ValueUpdated -= PassKeyValueResponse_ValueUpdated;
                        characteristic.ValueUpdated += PassKeyValueResponse_ValueUpdated;
                        await characteristic.StartUpdatesAsync();
                        break;

                    case CharacteristicType.TimeSynchronization:
                        break;

                    case CharacteristicType.Alert:
                        characteristic.ValueUpdated -= AlertValueUpdated;
                        characteristic.ValueUpdated += AlertValueUpdated;
                        await characteristic.StartUpdatesAsync();
                        break;

                    case CharacteristicType.CurrentGlucose:
                        characteristic.ValueUpdated -= CurrentGlucoseValueUpdated;
                        characteristic.ValueUpdated += CurrentGlucoseValueUpdated;
                        await characteristic.StartUpdatesAsync();
                        break;

                    case CharacteristicType.BulkGlucoseHistoryRequest:
                        break;

                    case CharacteristicType.BulkGlucoseHistoryResponseHeader:
                        characteristic.ValueUpdated -= HistoryResponseHeaderValueUpdated;
                        characteristic.ValueUpdated += HistoryResponseHeaderValueUpdated;
                        await characteristic.StartUpdatesAsync();
                        break;

                    case CharacteristicType.BulkGlucoseHistoryResponseBody:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                device.AddCharacteristic(type, characteristic);
                _logger.LogDebug($"Successful: add characteristics (GUID: {characteristic.Id}{type})");
                Debug.WriteLine($"Successful: add characteristics (GUID: {characteristic.Id} {type})");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                Debug.WriteLine("Exception In AddCharacteristic: " + e.Message);
                return false;
            }
        }

        #endregion

        #region Misc
        private void PublishAlert(SugarBeatAlert alert)
        {
            try
            {
                if (alert == null)
                    return;
                if (alert.Alerts == null)
                    return;
                if (alert.Alerts.Count <= 0)
                    return;
                foreach (var alertcode in alert.Alerts)
                {
                    switch (alertcode)
                    {
                        case AlertCode.ALERT_PATCH_NOT_CONNECTED:
                            _eventAggregator?.GetEvent<PatchConnectedEvent>().Publish(false);
                            _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Sensor not connected. Please connect.");
                            break;

                        case AlertCode.ALERT_PATCH_CONNECTED:
                            _eventAggregator?.GetEvent<PatchConnectedEvent>().Publish(true);
                            //Flag to check if the session expired message is sown to the user
                            IsSessionExpiryDisplayed = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                Debug.WriteLine("Exception In PublishAlert: " + ex.Message);
            }
        }

        public bool CheckHistorySyncDone()
        {
            return IsHistorySyncDone;
        }
        #endregion

        #region DB Calls and Session

        private bool IsSessionExpired()
        {
            try
            {
                //Check if its 14 hours old where sensor is expired
                if (ActiveSession != null)
                {
                    DateTime sessionStartTime = ActiveSession.Created;
                    // DateTime sessionStartTime = DateTime.Today;
                    if (DateTime.Now.Subtract(sessionStartTime).Hours >= 14)
                    {
                        if (!IsSessionExpiryDisplayed)
                        {
                            _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Sensor has expired. Please replace with a new one.");
                            IsSessionExpiryDisplayed = true;
                        }
                        ActiveSession = null;
                        return true;
                    }
                    else
                    {
                        getCurrentActiveSession();
                        if (ActiveSession == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (DateTime.Now.Subtract(sessionStartTime).Hours >= 14)
                            {
                                if (!IsSessionExpiryDisplayed)
                                {
                                    _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Sensor has expired. Please replace with a new one.");
                                    IsSessionExpiryDisplayed = true;
                                }
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsSessionExpired => exception: " + ex.Message);
                return false;
            }
        }

        private async void SaveAlert(SugarBeatAlert alert)
        {
            try
            {
                if (alert == null)
                    return;
                if (alert.Alerts == null)
                    return;
                if (alert.Alerts.Count <= 0)
                    return;

                foreach (var alertcode in alert.Alerts)
                {
                    if (alertcode != AlertCode.ALERT_NO_ALERT)
                    {
                        var _cts = new CancellationTokenSource();
                        SugarBeatAlertDto dto = new SugarBeatAlertDto();
                        dto.Code = alertcode;
                        dto.Created = alert.DateTime;
                        dto.CriticalCode = alert.CRCCode;
                        dto.FirmwareVersion = alert.SoftwareVersion;
                        dto.TransmitterId = _device.Name;
                        dto.BatteryVoltage = alert.BatteryVoltage;
                        await _sugarBeatAlertService.AddAsync(dto, _cts.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Debug.WriteLine("Exception In SaveAlert: " + ex.Message);
            }
        }

        private async Task<bool> SaveGlucose(SugarBeatGlucose glucose)
        {
            try
            {
                var _cts = new CancellationTokenSource();
                SugarBeatGlucoseDto dto = new SugarBeatGlucoseDto
                {
                    BatteryVoltage = glucose.Battery,
                    Created = glucose.DateTime,

                    FirmwareVersion = glucose.SoftwareVersion,
                    TransmitterId = _device.Name,
                    SensorValue = glucose.Glucose
                };

                var result = await _sugarBeatGlucosseService.AddAsync(dto, _cts.Token);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Debug.WriteLine("Exception In SaveGlucose: " + ex.Message);
                return false;
            }
        }

        private async void getCurrentActiveSession()
        {
            try
            {
                var _cts = new CancellationTokenSource();
                //Get Active Session from the db
                var apiresult = await _sugarBeatSessionService.GetActiveSessionAsync(DateTime.Now, _cts.Token);
                if (apiresult.Success)
                {
                    ActiveSession = apiresult.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Debug.WriteLine("Exception In getCurrentActiveSession: " + ex.Message);
            }
        }

        private async Task<DateTime?> getLastSyncTime()
        {
            try
            {
                //Get Last sync time from the db
                var _cts = new CancellationTokenSource();
                var result = await _settingsService.GetByAccountIdAsync(_cts.Token);
                if (result == null)
                {
                    return DateTime.Today.AddDays(-7);
                }

                var lastSyncTime = result?.Value?.LastSyncedTime;

                if (lastSyncTime.HasValue)
                {
                    if (lastSyncTime.Value < DateTime.Today.AddDays(-7) || lastSyncTime.Value == null)
                    {
                        lastSyncTime = DateTime.Today.AddDays(-7);
                    }
                }
                else
                {
                    lastSyncTime = DateTime.Today.AddDays(-7);
                }
                return lastSyncTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Debug.WriteLine("Exception In getLastSyncTime: " + ex.Message);
                return DateTime.Today.AddDays(-7);
            }
        }

        #endregion DB Calls
    }
}