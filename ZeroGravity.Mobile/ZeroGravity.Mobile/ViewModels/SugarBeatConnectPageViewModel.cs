using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class SugarBeatConnectPageViewModel : VmBase<ISugarBeatConnectPage, ISugarBeatConnectPageVmProvider, SugarBeatConnectPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ISugarBeatEatingSessionProvider _eatingSessionProvider;
        private string _password = string.Empty;
        private string _password1 = string.Empty;
        private string _password2 = string.Empty;
        private string _password3 = string.Empty;
        private string _password4 = string.Empty;
        private bool _sugerBeatSearch;
        private bool _sugerBeatActive;
        private bool _sugerBeatFail;
        private CancellationTokenSource _cts;
        private bool PatchWarmedUp = false;

        private const string HistorySyncingMessage = "Please wait while Miboko downloads your personal health tracking data from your device.";
        private const string PatchNotConnected = "Follow the simple steps on how to apply your wearable sensor in the Quick Start Guide included in your starter kit. \n \n Once your sensor is connected and warmed up, you can start tracking your health and wellbeing.";
        private const string SensorWarmingUpMessage = "Now you are all connected your device will require up to 20 minutes to complete the warm up process.";
        private const string YouMayEatNowMessage = "Your sensor is all warmed up and ready to start tracking how your body responds to sugar.\n \n Let’s find out exactly how the things you eat and do every day are uniquely affecting your metabolism. \n \n Each health tracking session is measured over a 4 hour period. Its important to track each time you eat. Click on ‘Activate’ to start tracking.";
        private const string HistorySyncingFailedMessage = "Downloading measurements from your Miboko transmitter failed.";
        private const string DisconnectedMessage = "Your Miboko transmitter has disconnected.\n \n Please ensure its close to your smart phone to ensure connectivity is maintained.";


        public SugarBeatConnectPageViewModel(IVmCommonService service, ISugarBeatConnectPageVmProvider provider, IEventAggregator eventAggregator, ISecureStorageService secureStorageService, ISugarBeatEatingSessionProvider sessionProvider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory, false)
        {
            _eventAggregator = eventAggregator;
            _secureStorageService = secureStorageService;
            LinkDeviceCommand = new Command(LinkDeviceExecute);
            AboutToEatCommand = new Command(onAboutToEat);
            _eatingSessionProvider = sessionProvider;
            //  EatingSessionTappedCommand = new Command(OnEatingSessionTappedCommand);
            ShowTodaysEatingSessionsCommand = new Command(OnShowEatingSessionTappedCommand);
            ShowHoldingPageCommand = new Command(OnShowEatingSessionTappedCommand);
            UnLinkDeviceCommand = new Command(UnlinkDeviceExecute);
        }


        #region "Properties

        private SugarBeatDevice selectedDevice = null;
        public SugarBeatDevice SelectedDevice
        {
            get => selectedDevice;
            set => SetProperty(ref selectedDevice, value);
        }

        private SugarBeatEatingSessionProxy _selectedEatingSession = null;
        public SugarBeatEatingSessionProxy SelectedEatingSession
        {
            get => _selectedEatingSession;
            set
            {
                _selectedEatingSession = null;
                SetProperty(ref _selectedEatingSession, value);
            }
        }
        private SugarBeatEatingSessionProxy _currentEatingSession = null;
        public SugarBeatEatingSessionProxy CurrentEatingSession
        {
            get => _currentEatingSession;
            set => SetProperty(ref _currentEatingSession, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private ObservableCollection<SugarBeatEatingSessionProxy> _todaysEatingSessions;
        public ObservableCollection<SugarBeatEatingSessionProxy> TodaysEatingSessions
        {
            get
            {
                if (_todaysEatingSessions == null)
                {
                    _todaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>();
                }
                return _todaysEatingSessions;
            }
            set => SetProperty(ref _todaysEatingSessions, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private SugarBeatAccessDetailsProxy _sugarBeatAccessDetails;
        public SugarBeatAccessDetailsProxy SugarBeatAccessDetails
        {
            get => _sugarBeatAccessDetails;
            set => SetProperty(ref _sugarBeatAccessDetails, value);
        }

        private bool _isLinked;
        public bool IsLinked
        {
            get => _isLinked;
            set => SetProperty(ref _isLinked, value);
        }

        private bool _isShowSession;
        public bool IsShowSession
        {
            get => _isShowSession;
            set => SetProperty(ref _isShowSession, value);
        }

        private string _macPart1;
        public string MacPart1
        {
            get => _macPart1;
            set => SetProperty(ref _macPart1, value);
        }

        private string _macPart2;
        public string MacPart2
        {
            get => _macPart2;
            set => SetProperty(ref _macPart2, value);
        }

        private string _macPart3;
        public string MacPart3
        {
            get => _macPart3;
            set => SetProperty(ref _macPart3, value);
        }

        private string _macPart4;
        public string MacPart4
        {
            get => _macPart4;
            set => SetProperty(ref _macPart4, value);
        }

        private string _macPart5;
        public string MacPart5
        {
            get => _macPart5;
            set => SetProperty(ref _macPart5, value);
        }

        private string _macPart6;
        public string MacPart6
        {
            get => _macPart6;
            set => SetProperty(ref _macPart6, value);
        }

        private string _displaymessage;
        public string DisplayMessage
        {
            get => _displaymessage;
            set => SetProperty(ref _displaymessage, value);
        }

        private bool _isSessionCreationAllowed;
        public bool IsSessionCreationAllowed
        {
            get => _isSessionCreationAllowed;
            set => SetProperty(ref _isSessionCreationAllowed, value);
        }

        private bool _showTodaysSessions;
        public bool ShowTodaysSessions
        {
            get => _showTodaysSessions;
            set => SetProperty(ref _showTodaysSessions, value);
        }

        private bool _isDeviceConnected;
        public bool IsDeviceConnected
        {
            get => _isDeviceConnected;
            set => SetProperty(ref _isDeviceConnected, value);
        }

        public bool SugerBeatSearch
        {
            get => _sugerBeatSearch;
            set => SetProperty(ref _sugerBeatSearch, value);
        }

        public bool SugerBeatActive
        {
            get => _sugerBeatActive;
            set => SetProperty(ref _sugerBeatActive, value);
        }
        public bool SugerBeatFail
        {
            get => _sugerBeatFail;
            set => SetProperty(ref _sugerBeatFail, value);
        }

        private bool _sugerBeatConnected;

        public bool SugerBeatConnected
        {
            get => _sugerBeatConnected;
            set => SetProperty(ref _sugerBeatConnected, value);
        }

        private bool _sugerBeatHeader;

        public bool SugerBeatHeader
        {
            get => _sugerBeatHeader;
            set => SetProperty(ref _sugerBeatHeader, value);
        }
        public string Password
        {
            get
            {
                _password = Password1 + Password2 + Password3 + Password4;
                return _password;
            }
            set => SetProperty(ref _password, value);
        }

        public string Password1
        {
            get => _password1;
            set => SetProperty(ref _password1, value);
        }
        public string Password2
        {
            get => _password2;
            set => SetProperty(ref _password2, value);
        }

        public string Password3
        {
            get => _password3;
            set => SetProperty(ref _password3, value);
        }

        public string Password4
        {
            get => _password4;
            set => SetProperty(ref _password4, value);
        }

        #endregion

        #region "Commands"

        public Command LinkDeviceCommand { get; }
        public Command UnLinkDeviceCommand { get; }
        public Command AboutToEatCommand { get; }
        public Command EatingSessionTappedCommand { get; }
        public Command ShowTodaysEatingSessionsCommand { get; }
        public Command ShowHoldingPageCommand { get; }

        #endregion

        #region "Navigation"

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                Title = AppResources.SugarBeatConnect_Title;
                var userName = await _secureStorageService.LoadString(SecureStorageKey.DisplayName);
                UserName = "Hello " + userName;
                IsLinked = Provider.IsDevicePaired;
                _eventAggregator?.GetEvent<BleDeviceDisconnectedStatusEvent>().Unsubscribe(OnDeviceLost);
                _eventAggregator?.GetEvent<BleDeviceDisconnectedStatusEvent>().Subscribe(OnDeviceLost);

                _eventAggregator?.GetEvent<PassKeyAuthFailedEvent>().Subscribe(OnPassKeyAuthFailedEvent);

                SugarBeatDeviceNavParams para = null;
                SugarBeatDeviceDisconnectedNavParams disconnectedPara = null;

                if (parameters != null)
                {
                    para = NavigationParametersHelper.GetNavigationParameters<SugarBeatDeviceNavParams>(parameters);
                    if (para == null)
                    {
                        disconnectedPara = NavigationParametersHelper.GetNavigationParameters<SugarBeatDeviceDisconnectedNavParams>(parameters);
                    }
                }
                if (para != null)
                {
                    if (SugarBeatAccessDetails == null)
                    {
                        SugarBeatAccessDetails = new SugarBeatAccessDetailsProxy();
                    }

                    //If Parameter is not null then the device is yet to be connected  else device is already connected and need to display holding page 

                    if (para != null)
                    {
                        if (para.Device == null)
                        {
                            Debug.WriteLine("Device was null in connect page-> OnNavigatedTo");
                            return;
                        }

                        if (!string.IsNullOrEmpty(para.Device?.Name))
                        {
                            SugarBeatAccessDetails.Address = para.Device?.Name;
                        }
                        if (!String.IsNullOrEmpty(para.Device?.MacAddress))
                        {
                            SugarBeatAccessDetails.Address = para.Device?.MacAddress;
                        }
                        selectedDevice = para?.Device;

                        Name = selectedDevice.Name;
                        Address = selectedDevice.MacAddress;
                        Debug.WriteLine("Update status active in connect page-> OnNavigatedTo");
                        UpdateDeviceStatus(SugerBeatPageStatus.Active);
                    }
                }
                else if (disconnectedPara != null)
                {
                    Debug.WriteLine("Update status disconnected Para in connect page-> OnNavigatedTo");
                    IsShowSession = false;
                   // UpdateDeviceStatus(SugerBeatPageStatus.Connected);
                    IsDeviceConnected = false;
                    _eventAggregator.GetEvent<HistorySyncStatusEvent>().Unsubscribe(OnHistorySynced);
                    _eventAggregator.GetEvent<HistorySyncStartedEvent>().Unsubscribe(OnHistorySyncStarted);
                    _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Unsubscribe(OnDeviceDisconnected);
                    _eventAggregator?.GetEvent<PatchConnectedEvent>().Unsubscribe(OnPatchConnected);

                    _eventAggregator.GetEvent<HistorySyncStatusEvent>().Subscribe(OnHistorySynced);
                    _eventAggregator.GetEvent<HistorySyncStartedEvent>().Subscribe(OnHistorySyncStarted);
                    _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Subscribe(OnDeviceDisconnected);
                    _eventAggregator?.GetEvent<PatchConnectedEvent>().Subscribe(OnPatchConnected);

                    _eventAggregator?.GetEvent<DeviceLostInMainPage>().Unsubscribe(OnDeviceLost);
                    _eventAggregator?.GetEvent<DeviceLostInMainPage>().Subscribe(OnDeviceLost);

                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);
                    //Get all eating sessions for the day
                    var cts = new CancellationTokenSource().Token;
                    var list = await _eatingSessionProvider.GetSugarBeatEatingSessionsAsync(DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay(), cts);
                    if (list != null)
                    {
                        if (list.Value != null)
                        {
                            if (list.Value.ToList().Count > 0)
                            {
                                TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>(list.Value.ToList());

                                if (TodaysEatingSessions?.Count > 0)
                                {
                                    ShowTodaysSessions = true;

                                    foreach (var sess in TodaysEatingSessions)
                                    {
                                        if (sess.StartTime <= DateTime.Now && sess.EndTime >= DateTime.Now)
                                        {
                                            _selectedEatingSession = sess;
                                            CurrentEatingSession = sess;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    ShowTodaysSessions = false;
                                }
                            }
                        }
                    }

                    //Check if history sync done ?
                    //if (this.Provider.CheckHistorySyncDone())
                    //{
                    //    //Checking for active patch connected session 
                    //    var IsPatchConnected = false;

                    //    var session = await this.Provider.GetActiveSugarBeatSessionFromDB(DateTime.Now, cts);

                    //    if (session?.Value != null)
                    //    {
                    //        IsPatchConnected = true;
                    //    }

                    //    //Check if patch disconnected 
                    //    if (IsPatchConnected && session?.Value != null)
                    //    {
                    //        //  Fetch Warmed up details
                    //        var res = await _eatingSessionProvider.IsSenorWarmedUp(session.Value.Id, cts);
                    //        if (res != null)
                    //        {
                    //            PatchWarmedUp = res.Value;
                    //        }

                    //        if (PatchWarmedUp)
                    //        {

                    //            if (CurrentEatingSession != null)
                    //            {
                    //                IsSessionCreationAllowed = false;
                    //            }
                    //            else
                    //            {
                    //                IsSessionCreationAllowed = true;
                    //            }

                    //            DisplayMessage = YouMayEatNowMessage;
                    //        }
                    //        else
                    //        {
                    //            DisplayMessage = SensorWarmingUpMessage;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        DisplayMessage = PatchNotConnected;
                    //    }
                    //}
                    //else
                    //{
                    //    DisplayMessage = HistorySyncingMessage;
                    //}
                    DisplayMessage = DisconnectedMessage;
                }
                else
                {
                  
                   
                        Debug.WriteLine("Update status connected in connect page-> OnNavigatedTo");
                        UpdateDeviceStatus(SugerBeatPageStatus.Connected);
                    
                    _eventAggregator.GetEvent<HistorySyncStatusEvent>().Unsubscribe(OnHistorySynced);
                    _eventAggregator.GetEvent<HistorySyncStatusEvent>().Subscribe(OnHistorySynced);

                    _eventAggregator.GetEvent<HistorySyncStartedEvent>().Unsubscribe(OnHistorySyncStarted);
                    _eventAggregator.GetEvent<HistorySyncStartedEvent>().Subscribe(OnHistorySyncStarted);

                    _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Unsubscribe(OnDeviceDisconnected);
                    _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Subscribe(OnDeviceDisconnected);

                    _eventAggregator?.GetEvent<PatchConnectedEvent>().Unsubscribe(OnPatchConnected);
                    _eventAggregator?.GetEvent<PatchConnectedEvent>().Subscribe(OnPatchConnected);

                    _eventAggregator?.GetEvent<DeviceLostInMainPage>().Unsubscribe(OnDeviceLost);
                    _eventAggregator?.GetEvent<DeviceLostInMainPage>().Subscribe(OnDeviceLost);

                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);

                    //Display holding page 
                    IsDeviceConnected = true;

                    //Get all eating sessions for the day
                    var cts = new CancellationTokenSource().Token;
                    var list = await _eatingSessionProvider.GetSugarBeatEatingSessionsAsync(DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay(), cts);
                    if (list != null)
                    {
                        if (list.Value != null)
                        {
                            if (list.Value.ToList().Count > 0)
                            {
                                TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>(list.Value.ToList());

                                if (TodaysEatingSessions?.Count > 0)
                                {
                                    ShowTodaysSessions = true;
                                    foreach (var sess in TodaysEatingSessions)
                                    {
                                        if (sess.StartTime <= DateTime.Now && sess.EndTime >= DateTime.Now)
                                        {
                                            _selectedEatingSession = sess;
                                            CurrentEatingSession = sess;
                                            IsSessionCreationAllowed = false;
                                            break;
                                        }
                                        else
                                        {
                                            IsSessionCreationAllowed = true;
                                        }
                                    }
                                }
                                else
                                {
                                    IsSessionCreationAllowed = true;
                                    ShowTodaysSessions = false;
                                }
                            }
                        }
                    }

                    //Check if history sync done ?
                    if (this.Provider.CheckHistorySyncDone())
                    {
                        //Checking for active patch connected session 
                        var IsPatchConnected = false;

                        var session = await this.Provider.GetActiveSugarBeatSessionFromDB(DateTime.Now, cts);

                        if (session?.Value != null)
                        {
                            IsPatchConnected = true;
                        }

                        //Check if patch disconnected 
                        if (IsPatchConnected && session?.Value != null)
                        {
                            //  Fetch Warmed up details
                            var res = await _eatingSessionProvider.IsSenorWarmedUp(session.Value.Id, cts);
                            if (res != null)
                            {
                                PatchWarmedUp = res.Value;
                            }

                            if (PatchWarmedUp)
                            {

                                if (CurrentEatingSession != null)
                                {
                                    IsSessionCreationAllowed = false;
                                }
                                else
                                {
                                    IsSessionCreationAllowed = true;
                                }

                                DisplayMessage = YouMayEatNowMessage;
                            }
                            else
                            {
                                DisplayMessage = SensorWarmingUpMessage;
                            }
                        }
                        else
                        {
                            DisplayMessage = PatchNotConnected;
                        }
                    }
                    else
                    {
                        DisplayMessage = HistorySyncingMessage;
                    }
                }
                if (!this.Provider.CheckSugarbeatConnected())
                {
                    Debug.WriteLine("Update status Disconnected in connect page-> OnNavigatedTo");
                    OnDeviceDisconnected(new Guid());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> navigated to : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }


        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            try
            {
                await Provider?.StopScanAsync();
                CancelPendingRequest(_cts);
                selectedDevice = null;
                base.OnNavigatedFrom(parameters);
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred in OnNavigatedFrom  Connect viewmodel", ex, ex.Message);
                Debug.WriteLine("Exception in OnNavigatedFrom -> navugated from : " + ex.Message + " Stacktrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region "Methods"

        private async void OnShowEatingSessionTappedCommand(object obj)
        {
            try
            {
                IsShowSession = !IsShowSession;
                //Refreshing Eating sessions
                if (IsShowSession)
                {
                    var cts = new CancellationTokenSource().Token;
                    var list = await _eatingSessionProvider.GetSugarBeatEatingSessionsAsync(DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay(), cts);
                    if (list != null)
                    {
                        if (list.Value != null)
                        {
                            if (list.Value.ToList().Count > 0)
                            {
                                TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>(list.Value.ToList());

                                if (TodaysEatingSessions.Count > 0)
                                {
                                    ShowTodaysSessions = true;
                                }
                                else
                                {
                                    ShowTodaysSessions = false;
                                }

                                foreach (var sess in TodaysEatingSessions)
                                {
                                    if (sess.StartTime <= DateTime.Now && sess.EndTime >= DateTime.Now)
                                    {
                                        _selectedEatingSession = sess;
                                        CurrentEatingSession = sess;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in OnShowEatingSessionTappedCommand " + ex.Message + " " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnSubmitExecute(object obj)
        {
            var navigationParams = new PageNavigationParams
            {
                PageName = ViewName.SugarBeatMainPage
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void OnPatchConnected(bool connected)
        {
            try
            {
                if (connected)
                {
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            //Checking for active patch connected session 
                            var IsPatchConnected = true;
                            var cts1 = new CancellationTokenSource().Token;
                            await Task.Delay(5000);
                            var session = await this.Provider.GetActiveSugarBeatSessionFromDB(DateTime.Now, cts1);
                            if (session != null)
                            {
                                if (session?.Value != null)
                                {
                                    Debug.WriteLine(session.Value.ToString());
                                    IsPatchConnected = true;
                                }
                                else
                                {
                                    IsPatchConnected = false;
                                }
                            }
                            else
                            {
                                IsPatchConnected = false;
                            }

                            //Check if patch disconnected 
                            if (IsPatchConnected)
                            {
                                var cts = new CancellationTokenSource().Token;
                                //  Fetch Warmed up details
                                var res = await _eatingSessionProvider.IsSenorWarmedUp(session.Value.Id, cts);

                                if (res != null)
                                {
                                    if (res.Value)
                                    {
                                        if (SelectedEatingSession != null)
                                        {
                                            IsSessionCreationAllowed = false;
                                            DisplayMessage = "";
                                        }
                                        else
                                        {
                                            IsSessionCreationAllowed = true;
                                            DisplayMessage = YouMayEatNowMessage;
                                        }
                                    }
                                    else
                                    {
                                        DisplayMessage = SensorWarmingUpMessage;
                                    }
                                }
                                else
                                {
                                    DisplayMessage = SensorWarmingUpMessage;
                                }
                            }
                            else
                            {
                                DisplayMessage = PatchNotConnected;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnPatchConnected (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);

                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                }
                else
                {
                    IsSessionCreationAllowed = false;
                    if (DisplayMessage != HistorySyncingMessage)
                        DisplayMessage = PatchNotConnected;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnPatchConnected : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async void OnGlucoseRecieved(SugarBeatGlucose obj)
        {
            if (obj == null)
            {
                return;
            }
            try
            {
                var cts = new CancellationTokenSource().Token;
                await Task.Delay(5000);
                var session = await this.Provider.GetActiveSugarBeatSessionFromDB(DateTime.Now, cts);
                if (session != null)
                {
                    if (session.Value != null)
                    {
                        var res = await _eatingSessionProvider.IsSenorWarmedUp(session.Value.Id, cts);
                        if (res != null)
                        {
                            PatchWarmedUp = res.Value;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                if (PatchWarmedUp)
                                {
                                    if (CurrentEatingSession != null)
                                    {
                                        if (CurrentEatingSession.EndTime < obj.DateTime)
                                        {
                                            CurrentEatingSession = null;
                                            IsSessionCreationAllowed = true;
                                        }
                                        else
                                        {
                                            IsSessionCreationAllowed = false;
                                        }
                                    }
                                    else
                                    {
                                        IsSessionCreationAllowed = true;
                                    }
                                    DisplayMessage = YouMayEatNowMessage;
                                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                                }
                                else
                                {
                                    DisplayMessage = SensorWarmingUpMessage;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnGlucoseRecieved (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                                Logger.LogCritical(ex, ex.Message);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnGlucoseRecieved : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnHistorySyncStarted(bool obj)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        DisplayMessage = HistorySyncingMessage;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnHistorySyncStarted (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);

                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnHistorySyncStarted : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnDeviceDisconnected(Guid obj)
        {
            try
            {
                if (this.Provider.CheckHistorySyncDone())
                {
                    Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        IsDeviceConnected = false;
                        DisplayMessage = DisconnectedMessage;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceDisconnected (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceDisconnected : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async void OnHistorySynced(bool obj)
        {
            try
            {
                if (obj)
                {
                    Debug.WriteLine("OnHistorySynced connect page viewmodel -> OnHistorySynced : " + obj);
                    var cts = new CancellationTokenSource().Token;
                    var list = await _eatingSessionProvider.GetSugarBeatEatingSessionsAsync(DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay(), cts);

                    //Enrollment should be done in case user connects to already warmed up device 

                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            if (list?.Value != null)
                            {
                                {
                                    TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>(list.Value);

                                    if (TodaysEatingSessions?.Count > 0)
                                    {
                                        ShowTodaysSessions = true;

                                        foreach (var sess in TodaysEatingSessions)
                                        {
                                            if (sess.StartTime <= DateTime.Now && sess.EndTime >= DateTime.Now)
                                            {
                                                _selectedEatingSession = sess;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ShowTodaysSessions = false;
                                    }
                                }
                            }

                            //Checking for active patch connected session 
                            var IsPatchConnected = false;
                            var cts1 = new CancellationTokenSource().Token;
                            var session = await this.Provider.GetActiveSugarBeatSessionFromDB(DateTime.Now, cts1);

                            if (session != null)
                            {
                                if (session.Value != null)
                                {
                                    IsPatchConnected = true;
                                }
                                else
                                {
                                    IsPatchConnected = false;
                                }
                            }
                            else
                            {
                                IsPatchConnected = false;
                            }


                            //Check if patch disconnected 
                            if (IsPatchConnected)
                            {
                                //  Fetch Warmed up details
                                var res = await _eatingSessionProvider.IsSenorWarmedUp(session.Value.Id, cts);

                                if (res != null)
                                {
                                    if (res.Value)
                                    {
                                        if (CurrentEatingSession != null)
                                        {
                                            IsSessionCreationAllowed = false;
                                            DisplayMessage = YouMayEatNowMessage;
                                        }
                                        else
                                        {
                                            IsSessionCreationAllowed = true;
                                            DisplayMessage = YouMayEatNowMessage;
                                        }
                                    }
                                    else
                                    {
                                        DisplayMessage = SensorWarmingUpMessage;
                                    }
                                }
                                else
                                {
                                    DisplayMessage = SensorWarmingUpMessage;
                                }
                            }
                            else
                            {
                                DisplayMessage = PatchNotConnected;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnHistorySynced : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            this.DisplayMessage = HistorySyncingFailedMessage;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnHistorySynced : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnHistorySynced : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async void LinkDeviceExecute()
        {
            try
            {
                if (selectedDevice == null)
                {
                    Debug.WriteLine("Device not select. Please select a device.");
                    return;
                }
                if (SugarBeatAccessDetails == null)
                    SugarBeatAccessDetails = new SugarBeatAccessDetailsProxy();

                SugarBeatAccessDetails.Password = Password;

                Debug.WriteLine("Update status search in connect page-> LinkDeviceExecute");
                UpdateDeviceStatus(SugerBeatPageStatus.Search);

                if (SugarBeatAccessDetails.Password.Length != 4)
                {
                    IsBusy = false;
                    await Service.DialogService.DisplayAlertAsync(
                        AppResources.SugarBeatConnect_Invalid_Password_Format_Title,
                        AppResources.SugarBeatConnect_Invalid_Password_Format_Message,
                        AppResources.Button_Ok);
                    return;
                }

                _cts = new CancellationTokenSource();

                // check if permission for location is granted (Android only)
                Debug.WriteLine("Checking if permission to location is granted...");
                if (!await CheckLocation())
                {
                    return;
                }

                Debug.WriteLine("Checking if bluetooth is enabled...");
                // check if bluetooth on device in enabled
                if (!Provider.IsBluetoothOn)
                {
                    Debug.WriteLine("Bluetooth is turned off");
                    await Service.DialogService.DisplayAlertAsync(AppResources.Bluetooth_Off_Title, AppResources.Bluetooth_Off_Message, AppResources.Button_Ok);
                    return;
                }

                Debug.WriteLine("Connecting to sugarBEAT device...");
                if (!await ConnectDeviceInternalAsync())
                {
                    // message box to inform user, no connection to sugarBEAT could be established
                    Device.BeginInvokeOnMainThread(async () =>
                        await Service.DialogService.DisplayAlertAsync(
                        AppResources.SugarBeatConnect_Pairing_Unsuccessful_Title,
                        AppResources.SugarBeatConnect_Pairing_Unsuccessful_Message,
                        AppResources.Button_Ok));

                    //Navigate to Scan page With Fail status
                    Debug.WriteLine("Device connection failed due to incorrect pin so navigating to scan page ");
                    //var navParams = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatScamPageNavParams() { IsRetry= true});
                    //await Service.NavigationService.NavigateAsync(ViewName.SugarBeatScanPage, navParams);
                    UpdateDeviceStatus(SugerBeatPageStatus.Active);
                    return;
                }
                else
                {
                    //   Debug.WriteLine("Update status connected in connect page-> LinkDeviceExecute");
                    //   UpdateDeviceStatus(SugerBeatPageStatus.Connected);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in SplitMacAddressForEntryFields : " + e.Message + " Stacktrace: " + e.StackTrace);
                Logger.LogError("An error occurred when trying to link sugarBEAT.", e, e.Message);
                IsBusy = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.SugarBeatConnect_Link_Error_Title, AppResources.SugarBeatConnect_Link_Error_Message, AppResources.Button_Ok);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in sugar beat connect page viewmodel -> LinkDeviceExecute (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void onAboutToEat()
        {
            try
            {
                IsSessionCreationAllowed = false;
                  //Create New Eating Session
                  var newSession = new SugarBeatEatingSessionProxy
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(4)
                };

                var cts = new CancellationTokenSource().Token;
                var result = await _eatingSessionProvider.AddSugarBeatEatingSessionAsync(newSession, cts);
                if (result.Success)
                {
                    SelectedEatingSession = result.Value;
                    if (SelectedEatingSession != null)
                    {
                        CurrentEatingSession = SelectedEatingSession;

                        _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                        _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);

                        this.TodaysEatingSessions.Add(result.Value);
                        var navParams = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatMainPageNavParams
                        { EatingSession = SelectedEatingSession, IsCurrentSession = true });
                        DisplayMessage = "";
                        IsSessionCreationAllowed = false;
                        await Service.NavigationService.NavigateAsync(ViewName.SugarBeatMainPage, navParams);
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await Service.DialogService.DisplayAlertAsync("Eating Session creation failed", "Eating Session creation failed. Please try again.", AppResources.Button_Ok);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> onAboutToEat (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);

                                Logger.LogCritical(ex, ex.Message);
                            }
                        });
                        DisplayMessage = YouMayEatNowMessage;
                       // IsSessionCreationAllowed = true;
                        return;
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Service.DialogService.DisplayAlertAsync("Eating Session creation failed", "Eating Session creation failed. Please try again.", AppResources.Button_Ok);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogCritical(ex, ex.Message);
                            Debug.WriteLine("Exception in sugar beat connect page viewmodel -> onAboutToEat (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);

                        }
                    });
                    DisplayMessage = YouMayEatNowMessage;
                    SelectedEatingSession = null;
                    IsSessionCreationAllowed = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> onAboutToEat : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        public async void OnEatingSessionTappedCommand(SugarBeatEatingSessionProxy selectedSession)
        {
            try
            {
                if (selectedSession == null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Service.DialogService.DisplayAlertAsync("Select eating session", "No eating session is selected. Please select one.", AppResources.Button_Ok);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnEatingSessionTappedCommand (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                            Logger.LogCritical(ex, ex.Message);
                        }
                    });
                    return;
                }
                else
                {
                    _selectedEatingSession = selectedSession;
                }
                bool isCurrentSession = false;
                if (CurrentEatingSession != null)
                {
                    if (SelectedEatingSession == CurrentEatingSession)
                    {
                        isCurrentSession = true;
                    }
                }
                else
                {
                    if (SelectedEatingSession.StartTime <= DateTime.Now && SelectedEatingSession.EndTime >= DateTime.Now)
                    {
                        CurrentEatingSession = SelectedEatingSession;
                        isCurrentSession = true;
                    }
                }
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatMainPageNavParams
                { EatingSession = SelectedEatingSession, IsCurrentSession = isCurrentSession });

                //  Send Selected Eating Session
                await Service.NavigationService.NavigateAsync(ViewName.SugarBeatMainPage, navParams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnEatingSessionTappedCommand : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async Task<bool> CheckLocation()
        {
            var locationGranted = true;

            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        locationGranted = await Provider.CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse()) == PermissionStatus.Granted;

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in sugar beat connect page viewmodel -> CheckLocation (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }

            if (!locationGranted)
            {
                Debug.WriteLine("Location must be enabled.");
                await Service.DialogService.DisplayAlertAsync(AppResources.Permission_Location_Required_Title, AppResources.Permission_Location_Required_Message, AppResources.Button_Ok);
            }
            else
            {
                Debug.WriteLine("Permission for location is granted.");
            }

            return locationGranted;
        }

        private async Task<bool> ConnectDeviceInternalAsync()
        {
            try
            {
                int attempts = 3;
                for (var i = 1; i <= attempts; i++)
                {
                    if (_cts?.IsCancellationRequested == true)
                    {
                        return false;
                    }

                    if (i > 1)
                    {
                        await Task.Delay(500);
                    }
                    // TODO check selectedDevice is null
                    if (!await Provider?.ConnectDeviceAsync(selectedDevice, _cts.Token))
                    {
                        Debug.WriteLine("ConnectDeviceAsync failed in  ConnectDeviceInternalAsync.");
                        return false;
                    }
                    else
                    {
                        if (!await InitAllCharacteristicInternalAsync(selectedDevice))
                        {
                            Debug.WriteLine("InitAllCharacteristicInternalAsync failed in ConnectDeviceInternalAsync.");
                        }
                        else
                        {
                            _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Unsubscribe(OnDeviceConnectionFailed);
                            _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Unsubscribe(OnRecievedConnectionStatus);
                            _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Subscribe(OnRecievedConnectionStatus);
                            _eventAggregator.GetEvent<DeviceConnectionFailedEvent>().Subscribe(OnDeviceConnectionFailed);

                            if (!await WritePasskeyAuthenticationInternalAsync(selectedDevice, SugarBeatAccessDetails.Password))
                            {
                                _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko Device connection failed. Invalid Pin/connection failure. Please try again.");
                                Debug.WriteLine("WritePasskeyAuthenticationInternalAsync failed in  ConnectDeviceInternalAsync.");
                                Debug.WriteLine("Update status connected in connect page-> ConnectDeviceInternalAsync");

                                UpdateDeviceStatus(SugerBeatPageStatus.Fail);
                                return false;
                            }
                            else
                            {
                                // TODO is need to call in UI Thread ?
                                _eventAggregator.GetEvent<HistorySyncStatusEvent>().Unsubscribe(OnHistorySynced);
                                _eventAggregator.GetEvent<HistorySyncStartedEvent>().Unsubscribe(OnHistorySyncStarted);
                                _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Unsubscribe(OnDeviceDisconnected);
                                _eventAggregator?.GetEvent<PatchConnectedEvent>().Unsubscribe(OnPatchConnected);

                                _eventAggregator.GetEvent<HistorySyncStatusEvent>().Subscribe(OnHistorySynced);
                                _eventAggregator.GetEvent<HistorySyncStartedEvent>().Subscribe(OnHistorySyncStarted);
                                _eventAggregator.GetEvent<BleDeviceDisconnectedEvent>().Subscribe(OnDeviceDisconnected);
                                _eventAggregator?.GetEvent<PatchConnectedEvent>().Subscribe(OnPatchConnected);

                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> ConnectDeviceInternalAsync : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
                return false;
            }
        }

        private void OnDeviceLost()
        {
            try
            {
                Debug.WriteLine("Device lost recieved in connect model");
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Debug.WriteLine("In connect page-> OnDeviceConnectionLost");
                        IsDeviceConnected = false;
                        DisplayMessage = DisconnectedMessage;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceConnectionFailed  (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceConnectionFailed : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async void OnRecievedConnectionStatus(bool isconnected)
        {
            try
            {
                if (!isconnected)
                {
                    IsDeviceConnected = false;
                    UpdateDeviceStatus(SugerBeatPageStatus.Active);
                }
                else
                {
                    IsDeviceConnected = true;
                    Debug.WriteLine("Connected to sugarBEAT device successfully.");

                    IsLinked = true;
                    Debug.WriteLine("Saving sugarBEAT credentials to device storage...");

                    if (SugarBeatAccessDetails != null && selectedDevice != null)
                    {
                        SugarBeatAccessDetails.Address = selectedDevice.Name;
                        await Provider?.SaveSugarBeatAccessDetailsAsync(SugarBeatAccessDetails);
                    }

                    Debug.WriteLine("Update status connected in connect page-> LinkDeviceExecute");
                    UpdateDeviceStatus(SugerBeatPageStatus.Connected);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnRecievedConnectionStatus : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnDeviceConnectionFailed()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   try
                   {
                       Debug.WriteLine("Update status fail in connect page-> OnDeviceConnectionFailed");
                       UpdateDeviceStatus(SugerBeatPageStatus.Active);
                       IsDeviceConnected = false;
                   }
                   catch (Exception ex)
                   {
                       Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceConnectionFailed  (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                       Logger.LogCritical(ex, ex.Message);
                   }
               });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceConnectionFailed : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnPassKeyAuthFailedEvent()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   try
                   {
                       Debug.WriteLine("Update status active in connect page-> OnPassKeyAuthFailedEvent");
                       _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Publish("Miboko Device connection failed. Invalid Pin. Please enter correct pin.");

                       UpdateDeviceStatus(SugerBeatPageStatus.Active);
                       IsDeviceConnected = false;
                   }
                   catch (Exception ex)
                   {
                       Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnPassKeyAuthFailedEvent (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                       Logger.LogCritical(ex, ex.Message);
                   }
               });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceConnectionFailed : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private async Task<bool> InitAllCharacteristicInternalAsync(SugarBeatDevice sugarBeatDevice)
        {
            try
            {
                if (_cts == null)
                {
                    _cts = new CancellationTokenSource();
                }

                var result = await Provider.InitAllCharacteristicAsync(sugarBeatDevice, _cts.Token);

                if (!result || _cts.IsCancellationRequested)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> InitAllCharacteristicInternalAsync : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> WritePasskeyAuthenticationInternalAsync(SugarBeatDevice sugarBeatDevice, string password)
        {
            try
            {
                var result = await Provider.WritePasskeyAuthenticationAsync(sugarBeatDevice, password, _cts.Token);
                if (!result || _cts.IsCancellationRequested)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> WritePasskeyAuthenticationInternalAsync : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
                return false;
            }
        }

        private void UpdateDeviceStatus(SugerBeatPageStatus sugerBeatPageStatus)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Debug.WriteLine(" UpdateDeviceStatus => " + sugerBeatPageStatus);
                        switch (sugerBeatPageStatus)
                        {
                            case SugerBeatPageStatus.Search:
                                SugerBeatFail = false;
                                SugerBeatActive = false;
                                SugerBeatConnected = false;
                                SugerBeatHeader = true;
                                SugerBeatSearch = true;
                                break;

                            case SugerBeatPageStatus.Active:
                                SugerBeatFail = false;
                                SugerBeatSearch = false;
                                SugerBeatActive = true;
                                SugerBeatHeader = true;
                                SugerBeatConnected = false;
                                break;

                            case SugerBeatPageStatus.Fail:
                                SugerBeatSearch = false;
                                SugerBeatActive = false;
                                SugerBeatFail = true;
                                SugerBeatHeader = true;
                                SugerBeatConnected = false;
                                break;

                            case SugerBeatPageStatus.Connected:
                                SugerBeatSearch = false;
                                SugerBeatActive = false;
                                SugerBeatFail = false;
                                SugerBeatHeader = false;
                                SugerBeatConnected = true;
                                break;

                            case SugerBeatPageStatus.Reconnect:
                                SugerBeatFail = false;
                                SugerBeatActive = false;
                                SugerBeatConnected = false;
                                SugerBeatHeader = true;
                                SugerBeatSearch = true;
                                break;

                            case SugerBeatPageStatus.Start:
                                SugerBeatFail = false;
                                SugerBeatActive = false;
                                SugerBeatConnected = false;
                                SugerBeatHeader = true;
                                SugerBeatSearch = true;
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("An error occurred in UpdateDeviceStatus  SB Connect viewmodel", ex, ex.Message);
                        Debug.WriteLine("Exception in UpdateDeviceStatus (BeginInvokeOnMainThread): " + ex.Message + " Stacktrace: " + ex.StackTrace);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in UpdateDeviceStatus : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogError("An error occurred in UpdateDeviceStatus SB Connect viewmodel", ex, ex.Message);
            }
        }

        private async void UnlinkDeviceExecute()
        {
            try
            {
                IsBusy = true;
                await Provider?.DeleteSugarBeatAccessDetailsAsync();

                //await Provider.DisconnectDeviceAsync(_sugarBeatDevice);
                await Provider?.CleanUpAsync(true); // matcht auch disconnect

                await Service?.NavigationService?.GoBackToRootAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in UnlinkDeviceExecute : " + e.Message + " Stacktrace: " + e.StackTrace);
                Logger.LogError("An error occurred when trying to unlink sugarBEAT.", e, e.Message);
                IsBusy = false;
                await Service?.DialogService?.DisplayAlertAsync(AppResources.SugarBeatConnect_Unlink_Error_Title, AppResources.SugarBeatConnect_Unlink_Error_Message, AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

    }
}