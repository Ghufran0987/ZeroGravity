using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Views;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class SugarBeatScanPageViewModel :
        VmBase<ISugarBeatScanPage, ISugarBeatScanPageVmProvider, SugarBeatScanPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource tryConnectingCancellationTokenSource;
        private bool _isSugerBeatConnect = false;
        private SugerBeatPageStatus SugarBeatStatus = SugerBeatPageStatus.Reconnect;

        public SugarBeatScanPageViewModel(IVmCommonService service, ISugarBeatScanPageVmProvider provider,
       ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;
            ConnectCommand = new DelegateCommand<SugarBeatDevice>(OnConnect);
            ScanCommand = new DelegateCommand(OnScan);
            RetryCommand = new DelegateCommand(OnRetry);
            BackCommand = new DelegateCommand(OnBack);
            CancelCommand = new DelegateCommand(OnCancel);
            ContinueCommand = new DelegateCommand(OnContinue);
            MetabolicHistoryCommand = new DelegateCommand(OnMetabolicHistory);
            SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        private void OnMetabolicHistory()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(
                    new MealNavParams(DateTime.UtcNow));
            Service.NavigationService.NavigateAsync(ViewName.MetabolicHealthTrackingHistoryPage, navigationParameters);
        }

        public bool IsSugerBeatConnect
        {
            get => _isSugerBeatConnect;
            set => SetProperty(ref _isSugerBeatConnect, value);
        }

        private bool _isSugerBeatDisConnect = true;

        public bool IsSugerBeatDisConnect
        {
            get => _isSugerBeatDisConnect;
            set => SetProperty(ref _isSugerBeatDisConnect, value);
        }

        private bool _sugerBeatHeader = true;

        public bool SugerBeatHeader
        {
            get => _sugerBeatHeader;
            set => SetProperty(ref _sugerBeatHeader, value);
        }

        private bool _sugerBeatConnectedHeader = false;

        public bool SugerBeatConnectedHeader
        {
            get => _sugerBeatConnectedHeader;
            set => SetProperty(ref _sugerBeatConnectedHeader, value);
        }

        private bool _sugerBeatInactive = false;

        public bool SugerBeatInactive
        {
            get => _sugerBeatInactive;
            set => SetProperty(ref _sugerBeatInactive, value);
        }

        private bool _sugerBeatSearch = false;

        public bool SugerBeatSearch
        {
            get => _sugerBeatSearch;
            set => SetProperty(ref _sugerBeatSearch, value);
        }

        private bool _sugerBeatActive = false;

        public bool SugerBeatActive
        {
            get => _sugerBeatActive;
            set => SetProperty(ref _sugerBeatActive, value);
        }

        public bool _sugarBeatReconnecting;

        public bool SugarBeatReconnecting
        {
            get => _sugarBeatReconnecting;
            set => SetProperty(ref _sugarBeatReconnecting, value);
        }

        private bool _sugerBeatFail = false;

        public bool SugerBeatFail
        {
            get => _sugerBeatFail;
            set => SetProperty(ref _sugerBeatFail, value);
        }

        private ImageSource _sugarBeatImageSource;

        public ImageSource SugarBeatImageSource
        {
            get => _sugarBeatImageSource;
            set => SetProperty(ref _sugarBeatImageSource, value);
        }

        public SugarBeatDevice SelectedDevice
        {
            get; set;
        }

        private ObservableCollection<SugarBeatDevice> _discoveredDevices;

        public ObservableCollection<SugarBeatDevice> DiscoveredDevices
        {
            get
            {
                if (_discoveredDevices == null)
                {
                    _discoveredDevices = new ObservableCollection<SugarBeatDevice>();
                }
                return _discoveredDevices;
            }
        }

        #region "Commands"

        public DelegateCommand<SugarBeatDevice> ConnectCommand { get; }
        public DelegateCommand ScanCommand { get; }
        public DelegateCommand<object> ConnectDeviceCommand { get; }
        public DelegateCommand RetryCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ContinueCommand { get; }

        public DelegateCommand MetabolicHistoryCommand { get; }
        #endregion

        #region "Navigation"
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            try
            {
                await Provider.StopScanAsync();
                _eventAggregator?.GetEvent<BlueTooothDeviceDiscoveredEvent>().Unsubscribe(OnBlueToothDeviceDiscovered);
                _eventAggregator?.GetEvent<ScanForAvailableDevicesCompleted>().Unsubscribe(OnScanForAvailableDevicesCompleted);

                _eventAggregator.GetEvent<BleDeviceReConnectedEvent>().Unsubscribe(OnDeviceConnected);
                _eventAggregator.GetEvent<BleDeviceReConnectedEvent>().Subscribe(OnDeviceConnected);

                base.OnNavigatedFrom(parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in main page viewmodel -> navigated from : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }


        private void OnDeviceConnected(SugarBeatDevice device)
        {
            OnConnect(device);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        #endregion

        #region "Methods"
        private async void OnCancel()
        {
            try
            {
                tryConnectingCancellationTokenSource?.Cancel();
                await Provider.StopScanAsync();
                await Provider.CleanUpAsync(true);
                await Provider.DeleteSugarBeatAccessDetailsAsync();
                UpdateDeviceStatus(SugerBeatPageStatus.Start);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }
        private void OnContinue()
        {
            try
            {
                IsSugerBeatConnect = true;
                IsSugerBeatDisConnect = false;
                Debug.WriteLine("Update status Reconnect in scan page-> OnContinue");

                UpdateDeviceStatus(SugerBeatPageStatus.Reconnect);

                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(DateTime.Now);

                // Find if Suragbeat is already connected
                var isConnected = Provider.CheckSugarbeatConnected();
                if (isConnected)
                {
                    IsSugerBeatConnect = true;
                    IsSugerBeatDisConnect = false;
                    Debug.WriteLine("Device was already connected while clicking on Glucose.");

                    Service.NavigationService.NavigateAsync(ViewName.SugarBeatConnectPage);
                }
                else
                {
                    Debug.WriteLine("Update status Reconnect in scan page-> OnNavigatedTo");
                    Debug.WriteLine("Device was not connected while clicking on Glucose trying to connected to saved device.");
                    _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Subscribe(DeviceReConnectedStatus);
                    _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Subscribe(OnDeviceConnectionFailed);

                    //Check connecting  for previously saved device
                    if (tryConnectingCancellationTokenSource == null)
                    {
                        tryConnectingCancellationTokenSource = new CancellationTokenSource();
                    }
                    Provider.TryReConnectToDeviceAsync(tryConnectingCancellationTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in scan page view model -> navugated to : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnBack()
        {
            Debug.WriteLine("Update status Start in scan page-> OnBack");
            UpdateDeviceStatus(SugerBeatPageStatus.Start);
        }

        private void OnRetry()
        {
            Debug.WriteLine("Update status search in scan page-> OnRetry");
            UpdateDeviceStatus(SugerBeatPageStatus.Search);
            OnScan();
        }

        private void OnScan()
        {
            ScanDeviceAsync();
        }

        private void ScanDeviceAsync()
        {
            try
            {
                Debug.WriteLine("ScanDeviceAsync device called.");
                _cancellationTokenSource = new CancellationTokenSource();

                Debug.WriteLine("Update status search in scan page-> ScanDeviceAsync");
                UpdateDeviceStatus(SugerBeatPageStatus.Search);

                DiscoveredDevices?.Clear();
                CheckLocation();

                _eventAggregator?.GetEvent<BlueTooothDeviceDiscoveredEvent>().Unsubscribe(OnBlueToothDeviceDiscovered);
                _eventAggregator?.GetEvent<ScanForAvailableDevicesCompleted>().Unsubscribe(OnScanForAvailableDevicesCompleted);

                _eventAggregator?.GetEvent<BlueTooothDeviceDiscoveredEvent>().Subscribe(OnBlueToothDeviceDiscovered);
                _eventAggregator?.GetEvent<ScanForAvailableDevicesCompleted>().Subscribe(OnScanForAvailableDevicesCompleted);

                Provider.StartScanForAvailableDevicesAsync(_cancellationTokenSource.Token, 30000);

                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
                IsBusy = false;
            }
        }

        /// <summary>
        /// Location access should be enabled for Bluetooth in Android paltform
        /// </summary>
        private void CheckLocation()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            var locationGranted = await Provider.CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse()) == PermissionStatus.Granted;
                            if (!locationGranted)
                            {
                                Debug.WriteLine("Location must be enabled.");
                                await Service.DialogService.DisplayAlertAsync(AppResources.Permission_Location_Required_Title, AppResources.Permission_Location_Required_Message, AppResources.Button_Ok);
                            }
                            else
                            {
                                Debug.WriteLine("Permission for location is granted.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async void OnConnect(SugarBeatDevice device)
        {
            try
            {
                // TODO Display Msg if Selected Device is Null ?
                if (device == null) return;

                await Provider.StopScanAsync();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        SelectedDevice = device;
                        var navParams = NavigationParametersHelper.CreateNavigationParameter(
                         new SugarBeatDeviceNavParams
                         {
                             Device = device
                         });
                        await Service.NavigationService.NavigateAsync(ViewName.SugarBeatConnectPage, navParams);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnDeviceConnectionFailed()
        {
            Debug.WriteLine("Update status Fail in scan page-> OnDeviceConnectionFailed");
            UpdateDeviceStatus(SugerBeatPageStatus.Fail);
        }

        private void DeviceReConnectedStatus(bool IsDeviceConnected)
        {
            try
            {
                tryConnectingCancellationTokenSource?.Dispose();
                if (IsDeviceConnected)
                {
                    Debug.WriteLine("Connected to a saved device hence navigating to SugarBeatMainPage");
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Service.NavigationService.NavigateAsync(ViewName.SugarBeatConnectPage);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                }
                else
                {
                    _eventAggregator?.GetEvent<BlueTooothDeviceDiscoveredEvent>().Subscribe(OnBlueToothDeviceDiscovered);
                    _eventAggregator?.GetEvent<ScanForAvailableDevicesCompleted>().Subscribe(OnScanForAvailableDevicesCompleted);

                    Debug.WriteLine("Could not Connect to a saved device hence navigating to taking back to scan");
                    Debug.WriteLine("Update status start in scan page-> DeviceReConnectedStatus");
                    UpdateDeviceStatus(SugerBeatPageStatus.Start);

                }
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Unsubscribe(DeviceReConnectedStatus);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnScanForAvailableDevicesCompleted()
        {
            try
            {
                Debug.WriteLine("OnScanForAvailableDevicesCompleted device called.");
                Debug.WriteLine("OnScanForAvailableDevicesCompleted -> count :" + this.DiscoveredDevices?.Count);

                if (this.DiscoveredDevices?.Count > 0)
                {
                    Debug.WriteLine("Update status active in scan page-> OnScanForAvailableDevicesCompleted");
                    UpdateDeviceStatus(SugerBeatPageStatus.Active);
                }
                else
                {
                    Debug.WriteLine("Update status fail in scan page-> OnScanForAvailableDevicesCompleted");
                    UpdateDeviceStatus(SugerBeatPageStatus.Fail);
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnBlueToothDeviceDiscovered(SugarBeatDevice obj)
        {
            try
            {
                if (obj == null) return;
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        var dev = new SugarBeatDevice(obj.Id, obj.Name, obj.Rssi);
                        Debug.WriteLine("OnBlueToothDeviceDiscovered  called: " + obj.Name);

                        DiscoveredDevices.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void UpdateDeviceStatus(SugerBeatPageStatus sugerBeatPageStatus)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Debug.WriteLine("UpdateDeviceStatus  called: " + sugerBeatPageStatus);

                    SugarBeatStatus = sugerBeatPageStatus;
                    SugerBeatHeader = true;
                    SugerBeatConnectedHeader = false;
                    try
                    {
                        switch (sugerBeatPageStatus)
                        {
                            case SugerBeatPageStatus.Start:
                                SugerBeatInactive = true;
                                SugerBeatFail = false;
                                SugerBeatSearch = false;
                                SugerBeatActive = false;
                                SugarBeatReconnecting = false;
                                SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MBK_Device.png");
                                break;

                            case SugerBeatPageStatus.Search:
                                SugerBeatInactive = false;
                                SugerBeatFail = false;
                                SugerBeatSearch = true;
                                SugerBeatActive = false;
                                SugarBeatReconnecting = false;
                                SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MBK_Device.png");
                                break;

                            case SugerBeatPageStatus.Active:
                                SugerBeatInactive = false;
                                SugerBeatFail = false;
                                SugerBeatSearch = false;
                                SugerBeatActive = true;
                                SugarBeatReconnecting = false;
                                SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MBK_Device.png");
                                SugerBeatHeader = false;
                                SugerBeatConnectedHeader = true;
                                break;

                            case SugerBeatPageStatus.Fail:
                                SugerBeatInactive = false;
                                SugerBeatFail = true;
                                SugerBeatSearch = false;
                                SugerBeatActive = false;
                                SugarBeatReconnecting = false;
                                SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MBK_Device.png");
                                break;

                            case SugerBeatPageStatus.Reconnect:
                                SugerBeatInactive = false;
                                SugerBeatFail = false;
                                SugerBeatSearch = false;
                                SugerBeatActive = false;
                                SugarBeatReconnecting = true;
                                SugarBeatImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MBK_Device.png");
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);
            }
        }

        #endregion

    }
}