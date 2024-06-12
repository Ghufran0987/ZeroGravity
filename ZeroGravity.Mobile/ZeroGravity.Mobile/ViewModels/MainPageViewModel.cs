using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MainPageViewModel : VmBase<IMainPage, IMainPageVmProvider, MainPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ITokenService _tokenService;

        private int _activityItemSum;

        private CancellationTokenSource _cts;

        private DateTime _currentDateTime;

        private DateTime _currentHistoryDateTime;

        private string _dateTimeString;
        private string _dateTimeFullString;

        private int _mealItemSum;

        private MyDayBadgeInformationProxy _myDayBadgeInformationProxy;

        private List<TrackedHistoryProxy> _trackedHistoryProxies;

        public MainPageViewModel(IVmCommonService service, IMainPageVmProvider provider, ILoggerFactory loggerFactory,
            IApiService apiService, ITokenService tokenService, IEventAggregator eventAggregator) : base(service,
            provider, loggerFactory, apiService)
        {
            _eventAggregator = eventAggregator;
            _tokenService = tokenService;
            Title = "Main Page";
            GetActivitiesCommand = new DelegateCommand(ActivitiesExecute);
            MealsSnacksCommand = new DelegateCommand(MealsSnacksExecute);
            GoToFastingCommand = new DelegateCommand(GetFastingExecute);
            ToDemoPageCommand = new DelegateCommand(OnToDemoPage);
            CalorieDrinksAlcoholCommand = new DelegateCommand(CalorieDrinksAlcohol);
            WaterIntakeCommand = new DelegateCommand(WaterIntake);
            WellbeingDataCommand = new DelegateCommand(WellbeingDataExecute);
            GoToMetabolicHealthCommand = new DelegateCommand(MetabolicHealthExecute);
            OnItemTappedCommand = new Command<object>(OnItemTapped);
            ToFeedbackCommand = new DelegateCommand(ToFeedbackExecute);
            ToHeartBeatCommand = new DelegateCommand(ToHeartBeatExecute);
            ToMeditationAreaCommand = new DelegateCommand(ToMeditationAreaExecute);
            ToEnduranceTrackingCommand = new DelegateCommand(ToEnduranceTrackingExecute);
            ToStreamingCommand = new DelegateCommand(ToStreamingExecute);
            PreviousHistoryDateCommand = new DelegateCommand(PreviousHistoryDate);
            NextHistoryDateCommand = new DelegateCommand(NextHistoryDate);

            TrackedHistoryProxies = new List<TrackedHistoryProxy>();

            UpdateTodayDate();
            _cts = new CancellationTokenSource();

            _eventAggregator.GetEvent<InitializeTabViewEvent>().Subscribe(LoadMainPageInformation);
        }

        private void UpdateTodayDate()
        {
            CurrentDateTime = DateTime.Today;
            CurrentHistoryDateTime = DateTime.Today;
            DateTimeString = DateTimeHelper.GetDateWithSuffix(CurrentDateTime);
            DateTimeFullString = DateTimeHelper.ToLocalDateZeroGravityFormat(CurrentDateTime);

        }

        public Command<object> OnItemTappedCommand { get; set; }
        public DelegateCommand PreviousHistoryDateCommand { get; set; }
        public DelegateCommand NextHistoryDateCommand { get; set; }

        public DateTime CurrentHistoryDateTime
        {
            get => _currentHistoryDateTime;
            set
            {
                if (_currentHistoryDateTime != value)
                {
                    SetProperty(ref _currentHistoryDateTime, value);
                    GetTrackedHistoryAsync(CurrentHistoryDateTime);
                }
            }
        }

        public List<TrackedHistoryProxy> TrackedHistoryProxies
        {
            get => _trackedHistoryProxies;
            set => SetProperty(ref _trackedHistoryProxies, value);
        }

        public string DateTimeString
        {
            get => _dateTimeString;
            set => SetProperty(ref _dateTimeString, value);
        }

        public string DateTimeFullString
        {
            get => _dateTimeFullString;
            set => SetProperty(ref _dateTimeFullString, value);
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                SetProperty(ref _currentDateTime, value);
                DateTimeString = DateTimeHelper.GetDateWithSuffix(_currentDateTime);
                DateTimeFullString = DateTimeHelper.ToLocalDateZeroGravityFormat(_currentDateTime);
            }
        }

        public int MealItemSum
        {
            get => _mealItemSum;
            set => SetProperty(ref _mealItemSum, value);
        }

        public int ActivityItemSum
        {
            get => _activityItemSum;
            set => SetProperty(ref _activityItemSum, value);
        }

        public MyDayBadgeInformationProxy MyDayBadgeInformationProxy
        {
            get => _myDayBadgeInformationProxy;
            set => SetProperty(ref _myDayBadgeInformationProxy, value);
        }

        public DelegateCommand GetActivitiesCommand { get; }
        public DelegateCommand GoToFastingCommand { get; }
        public DelegateCommand GoToMetabolicHealthCommand { get; }
        public DelegateCommand MealsSnacksCommand { get; }
        public DelegateCommand ToDemoPageCommand { get; }
        public DelegateCommand CalorieDrinksAlcoholCommand { get; }
        public DelegateCommand WaterIntakeCommand { get; }
        public DelegateCommand WellbeingDataCommand { get; }
        public DelegateCommand ToFeedbackCommand { get; }
        public DelegateCommand ToHeartBeatCommand { get; }
        public DelegateCommand ToMeditationAreaCommand { get; }
        public DelegateCommand ToEnduranceTrackingCommand { get; }
        public DelegateCommand ToStreamingCommand { get; }

        private void NextHistoryDate()
        {
            if (CurrentHistoryDateTime.AddDays(1) <= DateTime.Today)
            {
                CurrentHistoryDateTime = CurrentHistoryDateTime.AddDays(1);
            }
        }

        private void PreviousHistoryDate()
        {
            CurrentHistoryDateTime = CurrentHistoryDateTime.AddDays(-1);
        }

        private void OnItemTapped(object obj)
        {
            var itemEventArgs = obj as ItemTappedEventArgs;

            if (itemEventArgs?.ItemData is TrackedHistoryProxy proxy)
                switch (proxy.HistoryItemType)
                {
                    case HistoryItemType.Activity:
                        break;

                    case HistoryItemType.Breakfast:
                        break;

                    case HistoryItemType.Lunch:
                        break;

                    case HistoryItemType.Dinner:
                        break;

                    case HistoryItemType.HealthySnack:
                        break;

                    case HistoryItemType.UnhealthySnack:
                        break;

                    case HistoryItemType.CalorieDrinkAlcohol:
                        break;

                    case HistoryItemType.WaterIntake:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        private void MetabolicHealthExecute()
        {
            var navigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
               // PageName = ViewName.SugarBeatMainPage // placeholder page

                PageName = ViewName.SugarBeatScanPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);

            //_eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Subscribe(DeviceReConnectedStatus);

            ////Find if Suragbeat is already connected 

            //var isConnected = Provider.CheckSugarbeatConnected();
            //Debug.WriteLine("");
            //if (isConnected)
            //{
            //    Debug.WriteLine("Device was already connected while clicking on Glucose.");
            //    var navigationParams = new PageNavigationParams
            //    {
            //        DateTime = CurrentDateTime,
            //        //PageName = ViewName.SugarBeatMainPage // placeholder page

            //        PageName = ViewName.SugarBeatMainPage
            //    };
            //    _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
            //}
            //else
            //{
            //    Debug.WriteLine("Device was not connected while clicking on Glucose trying to connected to saved device.");

            //    //Check connecting  for previously saved device
            //    Provider.TryReConnectToDeviceAsync();
            //}
        }

        private void DeviceReConnectedStatus(bool IsDeviceConnected)
        {
            try
            {
                if (IsDeviceConnected)
                {
                    Debug.WriteLine("Connected to a saved device hence navigating to SugarBeatConnectPage");

                    var navigationParams = new PageNavigationParams
                    {
                        DateTime = CurrentDateTime,
                        //PageName = ViewName.SugarBeatMainPage // placeholder page

                        PageName = ViewName.SugarBeatConnectPage
                    };
                    _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
                }
                else
                {
                    Debug.WriteLine("Could not Connect to a saved device hence navigating to SugarBeatScanPage");

                    var navigationParams = new PageNavigationParams
                    {
                        DateTime = CurrentDateTime,
                        //PageName = ViewName.SugarBeatMainPage // placeholder page

                        PageName = ViewName.SugarBeatConnectPage
                    };
                    _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
                }
                _eventAggregator.GetEvent<BleDeviceReConnectedStatusEvent>().Unsubscribe(DeviceReConnectedStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.LogError(ex.Message, ex);
            }
        }


        private void ToFeedbackExecute()
        {
            var navigationParams = new FeedbackNavParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.ConsultingDetailPage,
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void ToHeartBeatExecute()
        {
            var navigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.HeartBeatPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void ToMeditationAreaExecute()
        {
            var navigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.MeditationAreaPage
            }; _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void ToEnduranceTrackingExecute()
        {
            var navigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.MyWeightPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void ToStreamingExecute()
        {
            var navigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.StreamingPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
        }

        private void LoadMainPageInformation(string viewName)
        {
            if (viewName.Equals(nameof(MainPageViewModel)))
            {
                GetMyDayBadgeInformationAsync(CurrentDateTime);
                GetTrackedHistoryAsync(CurrentHistoryDateTime);
            }
        }

        private void WellbeingDataExecute()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.WellbeingDataPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void WaterIntake()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.WaterIntakePage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void CalorieDrinksAlcohol()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.CalorieDrinksAlcoholPage
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _cts = new CancellationTokenSource();
            UpdateTodayDate();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }

        private Task GetMyDayBadgeInformationAsync(DateTime dateTime)
        {
            _ = Provider.GetMyDayBadgeInformationAsnyc(dateTime, _cts.Token).ContinueWith(async apiCallResult =>
              {
                  try
                  {
                      if (apiCallResult.Result.Success)
                      {
                          if (apiCallResult.Result?.Value != null)
                          {
                              MyDayBadgeInformationProxy = apiCallResult.Result.Value;

                              AddMainMealsToSum();

                              if (MyDayBadgeInformationProxy?.ActivityBadgeInformationProxy != null && MyDayBadgeInformationProxy?.ActivityBadgeInformationProxy != null)
                              {
                                  ActivityItemSum = MyDayBadgeInformationProxy.ActivityBadgeInformationProxy.ExerciseNumber +
                                              MyDayBadgeInformationProxy.ActivityBadgeInformationProxy.DayToDayNumber;
                              }
                          }
                      }
                      else
                      {
                          if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                              apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                          {
                              return;
                          }

                          await Service.DialogService.DisplayAlertAsync(AppResources.TabPage_MyDay,
                              apiCallResult.Result.ErrorMessage,
                              AppResources.Button_Ok);

                          return;
                      }
                  }
                  catch (Exception e)
                  {
                      Console.WriteLine(e);
                      Logger.LogError(e.Message, e);
                  }
              });
            return Task.CompletedTask;
        }

        private void AddMainMealsToSum()
        {
            //var breakfastAmount = MyDayBadgeInformationProxy.MealsBadgeInformationProxy.BreakfastAmount;
            //int age;
            //age = 10;
            //age = 20;
            //var lunchAmount = MyDayBadgeInformationProxy.MealsBadgeInformationProxy.LunchAmount;
            //var dinnerAmount = MyDayBadgeInformationProxy.MealsBadgeInformationProxy.DinnerAmount;

            //if (breakfastAmount != FoodAmountType.None && breakfastAmount != FoodAmountType.Undefined) MealItemSum++;

            //if (lunchAmount != FoodAmountType.None && lunchAmount != FoodAmountType.Undefined) MealItemSum++;

            //if (dinnerAmount != FoodAmountType.None && dinnerAmount != FoodAmountType.Undefined) MealItemSum++;
            if (MyDayBadgeInformationProxy?.MealsBadgeInformationProxy != null)
            {
                MealItemSum = MyDayBadgeInformationProxy.MealsBadgeInformationProxy.TotalAmount;
            }
        }

        private void GetTrackedHistoryAsync(DateTime dateTime)
        {
            IsBusy = true;
            try
            {
                Provider.GetTrackedHistoryAsnyc(dateTime, _cts.Token).ContinueWith(apiCallResult =>
                {
                    try
                    {
                        if (apiCallResult.Result.Success)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                TrackedHistoryProxies = apiCallResult.Result.Value;
                            });
                        }
                        else
                        {
                            if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                                apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                            {
                                IsBusy = false;
                                return;
                            }

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await Service.DialogService.DisplayAlertAsync(AppResources.TabPage_MyDay,
                                apiCallResult.Result.ErrorMessage,
                                AppResources.Button_Ok);
                            });

                            IsBusy = false;
                            return;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Logger.LogError(e.Message, e);
                    }
                    IsBusy = false;
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            // return Task.CompletedTask;
        }

        private void ActivitiesExecute()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.ActivitiesPage
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void MealsSnacksExecute()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.MealsSnacksPage
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void GetFastingExecute()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.FastingDataPage
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void OnToDemoPage()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = CurrentDateTime,
                PageName = ViewName.DemoPage
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }
    }
}