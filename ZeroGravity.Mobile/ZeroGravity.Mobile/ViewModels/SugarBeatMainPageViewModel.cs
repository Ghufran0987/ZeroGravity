using System;
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
using System.ComponentModel;
using ZeroGravity.Mobile.Contract.Enums;
using System.Collections.Generic;
using System.Linq;
using ZeroGravity.Mobile.Services;

namespace ZeroGravity.Mobile.ViewModels
{
    public class SugarBeatMainPageViewModel :
        VmBase<ISugarBeatMainPage, ISugarBeatMainPageVmProvider, SugarBeatMainPageViewModel>
    {
        private string _address;
        private PageNavigationParams _pageNavigationParams;
        private CancellationTokenSource _cts;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private SugarBeatEatingSessionProxy _selectedEatingSession;
        private readonly ISugarBeatEatingSessionProvider _eatingSessionProvider;
        private SugarBeatEatingSessionProxy _currentSession;

        public SugarBeatMainPageViewModel(IVmCommonService service, ISugarBeatMainPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator, ISecureStorageService secureStorageService, ISugarBeatEatingSessionProvider sessionProvider) : base(service, provider, loggerFactory)
        {

            UnLinkDeviceCommand = new DelegateCommand(UnlinkDeviceExecute);
            PlaceholderImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.glucose.png");
            _eventAggregator = eventAggregator;
            _secureStorageService = secureStorageService;
            _eatingSessionProvider = sessionProvider;
            ActualGlucoses = new ObservableCollection<ChartDataProxy>();
            CreateGreenZone();
        }

        #region "Commands"

        public DelegateCommand UnLinkDeviceCommand { get; }

        #endregion

        #region "Properties"

        private Color _metabolicScoreColor;

        public Color MetabolicScoreColor
        {
            get => _metabolicScoreColor;
            set => SetProperty(ref _metabolicScoreColor, value);
        }


        private ObservableCollection<ChartDataProxy> _hourlyTargetGlucoses;
        public ObservableCollection<ChartDataProxy> HourlyTargetGlucoses
        {
            get => _hourlyTargetGlucoses;
            set => SetProperty(ref _hourlyTargetGlucoses, value);
        }

        private ObservableCollection<ChartDataProxy> _targetGlucoses;
        public ObservableCollection<ChartDataProxy> TargetGlucoses
        {
            get => _targetGlucoses;
            set => SetProperty(ref _targetGlucoses, value);
        }

        private ObservableCollection<ChartDataProxy> _actualGlucoses;
        public ObservableCollection<ChartDataProxy> ActualGlucoses
        {
            get => _actualGlucoses;
            set => SetProperty(ref _actualGlucoses, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _subTitle;
        public string Subtitle
        {
            get => _subTitle;
            set => SetProperty(ref _subTitle, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private ImageSource _placeholderImageSource;
        public ImageSource PlaceholderImageSource
        {
            get => _placeholderImageSource;
            set => SetProperty(ref _placeholderImageSource, value);
        }

        private bool _showMetabolicScore;
        public bool ShowMetabolicScore
        {
            get => _showMetabolicScore;
            set => SetProperty(ref _showMetabolicScore, value);
        }

        private int _metabolicscore;
        public int Metabolicscore
        {
            get => _metabolicscore;
            set => SetProperty(ref _metabolicscore, value);
        }

        #endregion

        #region "Navigation"

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            try
            {
                if (_cts != null)
                    _cts.Cancel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in main page viewmodel -> navigated from : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void OnDeviceDisconnected()
        {
            try
            {
                Debug.WriteLine("Device lost recieved in main model");
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var navParams = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatDeviceDisconnectedNavParams
                        { IsDeviceDisconnected = true });
                        await Service.NavigationService.GoBackAsync(navParams);
                        //await Task.Delay(3000);
                        //_eventAggregator.GetEvent<DeviceLostInMainPage>().Publish();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in main page viewmodel ->  OnDeviceDisconnected BeginInvokeOnMainThread from: " + ex.Message + " Stacktrace: " + ex.StackTrace);

                        Logger.LogCritical(ex, ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnDeviceDisconnected : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);

                _eventAggregator.GetEvent<BleDeviceDisconnectedStatusEvent>().Unsubscribe(OnDeviceDisconnected);
                _eventAggregator.GetEvent<BleDeviceDisconnectedStatusEvent>().Subscribe(OnDeviceDisconnected);

                _eventAggregator.GetEvent<HistorySyncStatusEvent>().Unsubscribe(OnHistorySynced);
                _eventAggregator.GetEvent<HistorySyncStatusEvent>().Subscribe(OnHistorySynced);

                var userName = await _secureStorageService.LoadString(SecureStorageKey.DisplayName);
                UserName = "Hello " + userName;

                _cts = new CancellationTokenSource();

                //for testing
                //parameters = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatMainPageNavParams
                //{
                //    EatingSession = new SugarBeatEatingSessionProxy()
                //    { AccountId = 1054, StartTime = DateTime.Parse("2021-11-18 00:02:45.0000000"), EndTime=DateTime.Parse("2021-11-18 05:14:03.0000000"),MetabolicScore=30 },
                //    IsCurrentSession = true
                //});

                // Get SelectedSession from NavigationParameters
                if (parameters != null)
                {
                    // If Parameter is not null then the device is yet to be connected else device is already connected and need to display holding page 
                    SugarBeatMainPageNavParams para = NavigationParametersHelper.GetNavigationParameters<SugarBeatMainPageNavParams>(parameters);
                    if (para != null)
                    {
                        _selectedEatingSession = para.EatingSession;
                        if (_selectedEatingSession == null)
                        {
                            Debug.WriteLine("_selectedEatingSession was null in OnNavigatedTo");
                            return;
                        }
                        if (para.IsCurrentSession)
                        {
                            _currentSession = _selectedEatingSession;
                            Subtitle = "Eating Session started at " + _currentSession.StartTime.ToString(DisplayConversionService.GetTimeDisplayFormat());
                        }
                        else
                        {
                            _currentSession = null;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("parameters was null in OnNavigatedTo");
                        return;
                    }

                    PrepareMetabolicScoreGraphData();

                    var fromdate = _selectedEatingSession.StartTime;
                    var todate = _selectedEatingSession.EndTime;

                    _ = Provider.GetGlucoseForPeriodAsync(fromdate, todate, _cts.Token).ContinueWith(apiCallResult =>
                        {
                            if (apiCallResult?.Result != null)
                            {
                                if (apiCallResult.Result.Success)
                                {
                                    var actualdata = apiCallResult.Result.Value;
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        try
                                        {
                                            if (ActualGlucoses != null)
                                            {
                                                ActualGlucoses.Clear();
                                            }
                                            else
                                            {
                                                ActualGlucoses = new ObservableCollection<ChartDataProxy>();
                                            }
                                            if (actualdata == null)
                                            {
                                                return;
                                            }
                                            PrepareActualChartData(actualdata);

                                            // PrepareMetabolicScoreGraphData();
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine("Execption in OnNavigatedTo => updating glucose values " + ex.Message + "stacktrace " + ex.StackTrace);
                                            Logger.LogError(ex, ex.Message);
                                        }
                                    });
                                }
                                else
                                {
                                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                                             apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                                    {
                                        return;
                                    }
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        try
                                        {
                                            await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, apiCallResult.Result.ErrorMessage,
                                                 AppResources.Button_Ok);
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine("Execption in OnNavigatedTo =>display alert " + ex.Message + "stacktrace " + ex.StackTrace);
                                            Logger.LogError(ex, ex.Message);
                                        }
                                    });
                                }
                            }
                        });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, "No eating session is selected. Please select a eating session to display the results.",
                                 AppResources.Button_Ok);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Execption in OnNavigatedTo =>display alert " + ex.Message + "stacktrace " + ex.StackTrace);
                            Logger.LogError(ex, ex.Message);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Execption in OnNavigatedTo =>catch " + e.Message + "stacktrace " + e.StackTrace);

                Logger.LogError(e, e.Message);
            }
        }

        private void OnHistorySynced(bool obj)
        {
            try
            {
                if (_currentSession != null && obj )
                {

                    //  RefreshEatingSession();

                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Unsubscribe(OnGlucoseRecieved);
                    _eventAggregator?.GetEvent<SugarBeatGlucoseEvent>().Subscribe(OnGlucoseRecieved);


                    var cts = new CancellationTokenSource().Token;
                    Provider.GetGlucoseForPeriodAsync(_currentSession.StartTime, _currentSession.EndTime, cts).ContinueWith(apiCallResult =>
                    {
                        if (apiCallResult.Result != null)
                        {
                            if (apiCallResult.Result.Success)
                            {
                                var actualdata = apiCallResult.Result.Value;

                                if (actualdata == null)
                                {
                                    return;
                                }

                                PrepareActualChartData(actualdata);

                                RefreshEatingSession();
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execption in OnGlucoseRecieved: " + ex.Message + "stacktrace " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        #endregion

        #region "Methods"
        private void PrepareMetabolicScoreGraphData()
        {
            if (_selectedEatingSession == null) return;

            Device.BeginInvokeOnMainThread(() =>
             {
                 try
                 {
                     if (DateTime.Now.Subtract(_selectedEatingSession.StartTime).TotalHours > 1.5)
                     {
                         if (_selectedEatingSession.EndTime < DateTime.Now)
                         {
                             ShowMetabolicScore = true;
                         }

                         Metabolicscore = (int)Math.Floor(_selectedEatingSession.MetabolicScore);

                         if (Metabolicscore < 50)
                         {
                             MetabolicScoreColor = Color.Red;
                         }
                         else if (Metabolicscore >= 50 && Metabolicscore < 80)
                         {
                             MetabolicScoreColor = Color.FromHex("fec413");
                         }
                         else if (Metabolicscore >= 80)
                         {
                             MetabolicScoreColor = Color.Green;
                         }
                     }
                     else
                     {
                         MetabolicScoreColor = Color.Green;
                     }
                     CreateHourlyTargetglucose((int)DateTime.Now.Subtract(_selectedEatingSession.StartTime).TotalMinutes);
                 }
                 catch (Exception ex)
                 {
                     Debug.WriteLine("Execption in OnGlucoseRecieved => Metabolicscore update: " + ex.Message + "stacktrace " + ex.StackTrace);
                     Logger.LogError(ex, ex.Message);
                 }
             });
        }

        private void CreateGreenZone()
        {
            //  Get from current eating session
            TargetGlucoses = new ObservableCollection<ChartDataProxy>()
            {
                new ChartDataProxy() { XValue = 0, YValue = 3.628303147 },
                new ChartDataProxy() { XValue = 5,YValue = 3.683375994 },
                new ChartDataProxy() { XValue = 10,YValue = 3.75679378 },
                new ChartDataProxy() { XValue = 15,YValue = 3.852341149 },
                new ChartDataProxy() { XValue = 20,YValue = 3.973673491 },
                new ChartDataProxy() { XValue = 25,YValue = 4.123923929},
                new ChartDataProxy() { XValue = 30,YValue = 4.305232046},
                new ChartDataProxy() { XValue = 35,YValue = 4.518233325},
                new ChartDataProxy() { XValue = 40,YValue = 4.76156726},
                new ChartDataProxy() { XValue = 45,YValue = 5.031476582},
                new ChartDataProxy() { XValue = 50,YValue = 5.321575424},
                new ChartDataProxy() { XValue = 55,YValue = 5.622857309},
                new ChartDataProxy() { XValue = 60,YValue = 5.923992635},
                new ChartDataProxy() { XValue = 65,YValue = 6.211931001},
                new ChartDataProxy() { XValue = 70,YValue = 6.472780358},
                new ChartDataProxy() { XValue = 75,YValue = 6.692889269},
                new ChartDataProxy() { XValue = 80,YValue = 6.860019044},
                new ChartDataProxy() { XValue = 85,YValue = 6.964467312},
                new ChartDataProxy() { XValue = 90,YValue = 7 },
                new ChartDataProxy() { XValue = 95,YValue = 6.964467312},
                new ChartDataProxy() { XValue = 100,YValue = 6.860019044},
                new ChartDataProxy() { XValue = 105,YValue = 6.692889269},
                new ChartDataProxy() { XValue = 110,YValue = 6.472780358},
                new ChartDataProxy() { XValue = 115,YValue =6.211931001},
                new ChartDataProxy() { XValue = 120,YValue = 5.923992635},
                new ChartDataProxy() { XValue = 125,YValue = 5.622857309},
                new ChartDataProxy() { XValue = 130,YValue = 5.321575424},
                new ChartDataProxy() { XValue = 135,YValue = 5.031476582},
                new ChartDataProxy() { XValue = 140,YValue = 4.76156726},
                new ChartDataProxy() { XValue = 145,YValue = 4.518233325},
                new ChartDataProxy() { XValue = 150,YValue = 4.305232046},
                new ChartDataProxy() { XValue = 155,YValue = 4.123923929},
                new ChartDataProxy() { XValue = 160,YValue = 3.973673491},
                new ChartDataProxy() { XValue = 165,YValue = 3.852341149},
                new ChartDataProxy() { XValue = 170,YValue = 3.75679378},
                new ChartDataProxy() { XValue = 175,YValue = 3.683375994},
                new ChartDataProxy() { XValue = 180,YValue = 3.628303147},
                new ChartDataProxy() { XValue = 185,YValue = 3.587956711},
                new ChartDataProxy() { XValue = 190,YValue = 3.559079595},
                new ChartDataProxy() { XValue = 195,YValue = 3.538881488},
                new ChartDataProxy() { XValue = 200,YValue = 3.525071776},
                new ChartDataProxy() { XValue = 205,YValue = 3.515840328},
                new ChartDataProxy() { XValue = 210,YValue = 3.509805733},
                new ChartDataProxy() { XValue = 215,YValue = 3.505947478},
                new ChartDataProxy() { XValue = 220,YValue = 3.503534455},
                new ChartDataProxy() { XValue = 225,YValue = 3.502058017},
                new ChartDataProxy() { XValue = 230,YValue = 3.501174119},
                new ChartDataProxy() { XValue = 235,YValue = 3.500656315},
                new ChartDataProxy() { XValue = 240,YValue = 3.500359459},
            };
        }

        private void CreateHourlyTargetglucose(int minutes)
        {
            HourlyTargetGlucoses = new ObservableCollection<ChartDataProxy>(TargetGlucoses.ToList().Where(x => x.XValue <= minutes));
        }

        private void OnGlucoseRecieved(SugarBeatGlucose obj)
        {
            try
            {
                if (_currentSession != null && obj != null)
                {
                    if (DateTime.Now >= _currentSession.EndTime)
                    {
                        if (obj.DateTime > _currentSession.EndTime)
                        {
                            _currentSession = null;
                            return;
                        }
                    }
                    //  RefreshEatingSession();

                    var cts = new CancellationTokenSource().Token;
                    Provider.GetGlucoseForPeriodAsync(_currentSession.StartTime, _currentSession.EndTime, cts).ContinueWith(apiCallResult =>
                    {
                        if (apiCallResult.Result != null)
                        {
                            if (apiCallResult.Result.Success)
                            {
                                var actualdata = apiCallResult.Result.Value;

                                if (actualdata == null)
                                {
                                    return;
                                }

                                PrepareActualChartData(actualdata);

                                RefreshEatingSession();
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execption in OnGlucoseRecieved: " + ex.Message + "stacktrace " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }

        private void RefreshEatingSession()
        {
            //Refresh current session
            var cs = new CancellationTokenSource().Token;
            _eatingSessionProvider.GetSugarBeatEatingSessionAsync(_currentSession.Id, cs).ContinueWith(session =>
            {
                try
                {
                    if (session != null)
                    {
                        if (session?.Result?.Value != null)
                        {
                            _currentSession = session.Result.Value;
                            _selectedEatingSession = _currentSession;
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Execption in RefreshEatingSession => currentSession update: " + ex.Message + "stacktrace " + ex.StackTrace);
                    Logger.LogError(ex, ex.Message);
                }
                PrepareMetabolicScoreGraphData();
            });
        }

        private void PrepareActualChartData(List<SugarBeatGlucoseProxy> values)
        {
            // Commenting as we are not showing actual data https://github.com/Prestine-Technologies-Pvt-Ltd/miboko/issues/304
            // return;

            if (values == null || _selectedEatingSession == null)
            {
                return;
            };

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    // Set ssession date                   
                    DateTime startTime = _selectedEatingSession.StartTime;
                    if (ActualGlucoses != null)
                    {
                        ActualGlucoses.Clear();
                    }
                    else
                    {
                        ActualGlucoses = new ObservableCollection<ChartDataProxy>();
                    }

                    var list = new List<ChartDataProxy>();
                    foreach (var item in values)
                    {
                        var min = GetMinutes(startTime, item.DateTime);
                        var glo = item.Glucose;

                        // Limit Max Glucose Value If not in Range
                        if (item.Glucose > 9)
                        {
                            glo = 9;
                        }
                        if (item.Glucose < 3.5)
                        {
                            glo = 3.5;
                        }
                        list.Add(new ChartDataProxy() { XValue = min, YValue = glo });
                    }


                    // Ensure minumum 2 points are there for Normilization
                    if (list.Count > 2)
                    {
                        var lastPoint = list.LastOrDefault();
                        var normalizedList = NormailzeDrops(list, lastPoint);
                        if (normalizedList != null)
                        {
                            foreach (var item in normalizedList)
                            {
                                ActualGlucoses.Add(item);
                            }
                        }
                        else
                        {
                            foreach (var item in list)
                            {
                                ActualGlucoses.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            ActualGlucoses.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in main page viewmodel -> PrepareActualChartData : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                    Logger.LogCritical(ex, ex.Message);
                }
            });
        }

        /// <summary>
        /// Normalizing Drop Points for the Curve
        /// </summary>
        /// <param name="range"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        //private List<ChartDataProxy> NormailzeDrops(List<ChartDataProxy> range, ChartDataProxy current)
        //{

        //    try
        //    {
        //        if (current == null || range == null) return null;
        //        var glo = current.YValue;

        //        // Find Peak after 2 hours GreenZone Graph Peak Point
        //        var greenZonePeakPointTime = 30;
        //        var greenZonePeakPointEndTime = 150;

        //        // Wait for minumum 5 points after greenZonePeakPointTime
        //        if (current.XValue >= greenZonePeakPointTime)
        //        {
        //            // Find Max Point in next 5 points after greenZonePeakPointTime
        //            var countPoints = range.Where(x => x.XValue >= greenZonePeakPointTime && x.XValue <= greenZonePeakPointEndTime).ToList();
        //            if (countPoints?.Count > 5)
        //            {
        //                // Find Max Point
        //                var max = countPoints.Max(x => x.YValue);
        //                var maxPoint = countPoints.Where(x => x.YValue == max).FirstOrDefault();

        //                if (maxPoint == null || max <= 0) return null;

        //                // Wait for another 5 point from Max Point to calculate rate of fall
        //                var maxNextPoints = range.Where(x => x.XValue > maxPoint.XValue).ToList();
        //                if (maxNextPoints?.Count() > 5)
        //                {
        //                    // Check for Rate of Fall in next 5 points from max points if not found skip
        //                    double previousMax = maxPoint.YValue;
        //                    bool found = false;
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        double point = maxNextPoints[i].YValue;
        //                        if (point <= previousMax)
        //                        {
        //                            previousMax = point;
        //                            if (i == 4)
        //                            {
        //                                // All 5 points are in the falling range
        //                                found = true;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // No next falling point found so skip 
        //                            break;
        //                        }
        //                    }
        //                    if (found)
        //                    {
        //                        // Found Falling points so Calulate Avg Fall Rate
        //                        var avgFallRate = maxNextPoints.Take(5).Average(x => x.YValue);
        //                        if (avgFallRate <= 0) return null;
        //                        // Get all points after max point and normalize
        //                        var list = range.Where(x => x.XValue > maxPoint.XValue).ToList();
        //                        var prviousPoint = maxPoint.YValue;
        //                        for (int i = 0; i < list?.Count; i++)
        //                        {
        //                            // check next point if fall under fall of rate else correct it
        //                            var point = list[i];
        //                            if (point.YValue > (prviousPoint - avgFallRate))
        //                            {
        //                                point.YValue = prviousPoint - avgFallRate;
        //                            }
        //                            prviousPoint = point.YValue;
        //                        }
        //                        return list;
        //                    }
        //                }
        //            }
        //        }             
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, ex.Message);
        //    }
        //    return null;
        //}

        private List<ChartDataProxy> NormailzeDrops(List<ChartDataProxy> range, ChartDataProxy current)
        {
            try
            {
                if (current == null || range == null) return null;

                // Look for peak in actual glucose values between the 30 and 150 minute mark
                var greenZonePeakCheckValues = range.Where(x => x.XValue >= 30 && x.XValue <= 150).ToList();
                var ordered = greenZonePeakCheckValues.OrderByDescending(x => x.YValue).ToList();
                var peak = ordered.First();
                var indexOfPeak = range.IndexOf(peak);

                // Wait for minumum 5 points after prek
                if (current.XValue >= peak.XValue)
                {
                    // Find Max Point in next 5 points after peak
                    var pointsAfterPeak = range.Where(x => x.XValue >= peak.XValue).ToList();
                    if (pointsAfterPeak?.Count > 5)
                    {
                        // Check for Rate of Fall in next 5 points from max points if not found skip

                        double sumDifferencesPostPeak = 0;

                        for (int i = 1; i < 5; i++)
                        {
                            sumDifferencesPostPeak += pointsAfterPeak[i].YValue - pointsAfterPeak[i - 1].YValue;
                        }

                        var averageRateOfChangePostPeak = sumDifferencesPostPeak / 4;

                        if (averageRateOfChangePostPeak < 0)
                        {
                            // debug logging showing values before modification
                            for (int i = 0; i < range.Count; i++)
                            {
                                Logger.LogDebug(range[i].XValue + "," + range[i].YValue);
                            }

                            // the glucose values are falling after the identified peak.

                            var updateList = range.ToList();

                            // update check the y values after the 5 points used to find the rate of fall and correct them if
                            // needed to ensure they stay within that rate of fall.

                            for (int i = indexOfPeak + 5; i < updateList.Count; i++)
                            {
                                if (updateList[i].YValue - updateList[i - 1].YValue > averageRateOfChangePostPeak)
                                {
                                    updateList[i].YValue = updateList[i - 1].YValue + averageRateOfChangePostPeak;
                                }
                            }

                            // debug logging showing values after modification
                            Logger.LogDebug("sumDifferencesPostPeak: " + sumDifferencesPostPeak);
                            Logger.LogDebug("averageRateOfChangePostPeak: " + averageRateOfChangePostPeak);
                            Logger.LogDebug("peak x: " + peak.XValue + ", y: " + peak.YValue);
                            Logger.LogDebug("peak index: " + indexOfPeak);

                            for (int i = 0; i < range.Count; i++)
                            {
                                Logger.LogDebug(updateList[i].XValue + "," + updateList[i].YValue);
                            }

                            return updateList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execption in NormailzeDrops: " + ex.Message + "stacktrace " + ex.StackTrace);
                Logger.LogError(ex, ex.Message);
            }
            return null;
        }

        private int GetMinutes(DateTime startTime, DateTime currentTime)
        {
            if (currentTime != null && startTime != null)
            {
                var diff = currentTime.Subtract(startTime);
                return (int)diff.TotalMinutes;
            }
            else
            {
                return 0;
            }
        }

        private async void UnlinkDeviceExecute()
        {
            try
            {
                IsBusy = true;
                await Provider.DeleteSugarBeatAccessDetailsAsync();
                await Provider.CleanUpAsync(true);
                await Service.NavigationService.GoBackToRootAsync();
            }
            catch (Exception e)
            {
                Logger.LogError("An error occurred when trying to UnlinkDeviceExecute: ", e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.SugarBeatConnect_Unlink_Error_Title, AppResources.SugarBeatConnect_Unlink_Error_Message, AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}