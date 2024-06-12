using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Models;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ActivitiesPageViewModel : VmBase<IActivitiesPage, IActivitiesPageVmProvider, ActivitiesPageViewModel>
    {
        private DateTime _actualDateTime;
        private CancellationTokenSource _cts;
        private ExerciseActivityProxy _exerciseActivityProxy;
        private IEducationalInfoProvider educationalInfoProvider;
        private IEnumerable<ZgButtonGroupItem> _groupItems;

        private List<IntegrationDataProxy> _integrationDataProxies;
        private ObservableCollection<ComboBoxItem> _intensitySource;
        private bool _isManual;
        private bool _isSync;
        private double _maxDuration;
        private double _minDuration;

        private ZgButtonGroupItem _selectedGroupItem;

        private double _summary;

        private string _summaryText;
        private string _summaryTextHr;
        private string _summaryTextMin;

        private int _durationHr;

        public int DurationHr
        {
            get => _durationHr;
            set => SetProperty(ref _durationHr, value);
        }

        private int _durationMin;

        public int DurationMin
        {
            get => _durationMin;
            set => SetProperty(ref _durationMin, value);
        }

        public ActivitiesPageViewModel(IVmCommonService service, IActivitiesPageVmProvider provider,
            ILoggerFactory loggerFactory, IEducationalInfoProvider _educationalInfoProvider) : base(service, provider, loggerFactory)
        {
            IntegrationDataProxies = new List<IntegrationDataProxy>();
            educationalInfoProvider = _educationalInfoProvider;
            ManualCommand = new DelegateCommand(() =>
            {
                IsManual = true;
                IsSync = !IsManual;
            });

            SyncCommand = new DelegateCommand(() =>
            {
                IsManual = false;
                IsSync = !IsManual;
            });

            SaveCommand = new DelegateCommand(Save);

            OnItemTappedCommand = new Command<object>(OnItemTapped);
            ActivitiesImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Activities.png");
        }

        private ImageSource _activitiesImageSource;

        public ImageSource ActivitiesImageSource
        {
            get => _activitiesImageSource;
            set => SetProperty(ref _activitiesImageSource, value);
        }

        public Command<object> OnItemTappedCommand { get; set; }

        public DateTime TargetDateTime { get; set; }

        public DelegateCommand ManualCommand { get; }
        public DelegateCommand SyncCommand { get; }
        public DelegateCommand SaveCommand { get; }

        public List<IntegrationDataProxy> IntegrationDataProxies
        {
            get => _integrationDataProxies;
            set => SetProperty(ref _integrationDataProxies, value);
        }

        public ObservableCollection<ComboBoxItem> IntensitySource
        {
            get => _intensitySource;
            set => SetProperty(ref _intensitySource, value);
        }

        public double MinDuration
        {
            get => _minDuration;
            set => SetProperty(ref _minDuration, value);
        }

        public double MaxDuration
        {
            get => _maxDuration;
            set => SetProperty(ref _maxDuration, value);
        }

        public bool IsManual
        {
            get => _isManual;
            set => SetProperty(ref _isManual, value);
        }

        public bool IsSync
        {
            get => _isSync;
            set => SetProperty(ref _isSync, value);
        }

        public ExerciseActivityProxy ExerciseActivityProxy
        {
            get => _exerciseActivityProxy;
            set => SetProperty(ref _exerciseActivityProxy, value);
        }

        public double Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public string SummaryText
        {
            get => _summaryText;
            set => SetProperty(ref _summaryText, value);
        }

        public string SummaryTextHr
        {
            get => _summaryTextHr;
            set => SetProperty(ref _summaryTextHr, value);
        }

        public string SummaryTextMin
        {
            get => _summaryTextMin;
            set => SetProperty(ref _summaryTextMin, value);
        }

        public IEnumerable<ZgButtonGroupItem> GroupItems
        {
            get => _groupItems;
            set => SetProperty(ref _groupItems, value);
        }

        public ZgButtonGroupItem SelectedGroupItem
        {
            get => _selectedGroupItem;
            set => SetProperty(ref _selectedGroupItem, value);
        }

        private async void OnItemTapped(object obj)
        {
            try
            {
                var itemEventArgs = obj as ItemTappedEventArgs;

                if (itemEventArgs?.ItemData is IntegrationDataProxy proxy)
                {
                    if (proxy.Id == -100)
                    {
                        var navParams = NavigationParametersHelper.CreateNavigationParameter(new ActivityNavParams()
                        { TargetDateTime = _actualDateTime });

                        await Service.NavigationService.NavigateAsync(ViewName.StepCountPage, navParams);
                    }
                    else
                    {
                        GetActivityDataAsync(proxy, _actualDateTime);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error when {nameof(OnItemTapped)} in {nameof(ActivitiesPageViewModel)}.\n{e}\n{e.Message}");
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
        }

        private async void GetActivityDataAsync(IntegrationDataProxy integrationDataProxy, DateTime targetDate)
        {
            //IsBusy = true;

            //var apiCallResult =
            //    await

            Provider.GetActivitySyncDataAsnyc(integrationDataProxy, targetDate, _cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    var syncActivityProxies = apiCallResult.Result.Value;

                    var navParams = NavigationParametersHelper.CreateNavigationParameter(new SyncActivityNavParams
                    { TargetDateTime = targetDate, ExerciseActivityProxies = syncActivityProxies });

                    await Service.NavigationService.NavigateAsync(ViewName.ActivitySyncDetailPage, navParams);
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation)
                    {
                        IsBusy = false;
                        return;
                    }

                    await Service.DialogService.DisplayAlertAsync(AppResources.ActivitySyncOverview_Title,
                            apiCallResult.Result.ErrorMessage,
                            AppResources.Button_Ok);

                    IsBusy = false;
                    return;
                }

                IsBusy = false;
            });
        }

        private async void Save()
        {
            try
            {
                IsBusy = true;
                if (!await ValidateToken()) return;

                _cts = new CancellationTokenSource();

                if (DurationHr == 0 && DurationMin == 0)
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, "Please enter activity duration to save.",
                   AppResources.Button_Ok);
                    return;
                }

                TimeSpan timeSpan = new TimeSpan(DurationHr, DurationMin, 0);

                ExerciseActivityProxy.Duration = timeSpan.TotalMinutes;

                var saveResult = await Provider.SaveActivityAsync(ExerciseActivityProxy, _cts.Token);

                if (saveResult.Success)
                {
                    //await CreateSummary(DateTime.Now);

                    await Service.NavigationService.GoBackAsync();
                }
                else
                {
                    if (saveResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                        saveResult.ErrorReason == ErrorReason.TimeOut)
                        return;

                    IsBusy = false;
                    await Service.DialogService.DisplayAlertAsync(AppResources.Activities_Title,
                        saveResult.ErrorMessage,
                        AppResources.Button_Ok);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                IsBusy = true;

                _cts = new CancellationTokenSource();

                base.OnNavigatedTo(parameters);

                //  if (!await ValidateToken()) return;

                var navParams = NavigationParametersHelper.GetNavigationParameters<ActivityNavParams>(parameters);
                if (navParams == null)
                {
                    _actualDateTime = DateTime.Now;

                    //IntegrationDataProxies = new List<IntegrationDataProxy>();
                }
                else
                {
                    _actualDateTime = navParams.TargetDateTime.Date;
                    _actualDateTime = _actualDateTime.Add(DateTime.Now.TimeOfDay);
                }

                GetLinkedIntegrationsAsync();

                //Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_actualDateTime.Date);

                if (GroupItems == null || !GroupItems.Any())
                {
                    IsManual = true;
                    IsSync = !IsManual;

                    InitSwitchButton();
                }

                CreateIntensitySource();
                CreateExerciseActivityProxy(_actualDateTime);

                CreateSummary(_actualDateTime);

                var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.Activities);
                if (showOverlay)
                {
                    IsLoadingImageBusy = true;
                    OpenOverlay();
                    educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.Activities).ContinueWith(async apiCallEducation =>
                    {
                        if (apiCallEducation.Result.Success && apiCallEducation.Result.Value != null)
                        {
                            var educationResult = apiCallEducation.Result;
                            ProductImage = new UriImageSource { Uri = new Uri(educationResult.Value.ImageUrl) };
                        }
                        IsLoadingImageBusy = false;
                    });
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Activities);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.Activities);
        }

        private async void GetLinkedIntegrationsAsync()
        {
            IsBusy = true;
            Provider.GetLinkedIntegrationListAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    var tempDataProxies = apiCallResult.Result.Value;
                    tempDataProxies.Insert(0, new IntegrationDataProxy
                    {
                        Id = -100,
                        IntegrationType = 0,
                        IsLinked = true,
                        Name = AppResources.Activities_Stepcounter
                    });
                    IntegrationDataProxies = tempDataProxies;
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
                        await Service.DialogService.DisplayAlertAsync(AppResources.ActivitySyncOverview_Title,
                        apiCallResult.Result.ErrorMessage,
                        AppResources.Button_Ok);
                    });
                    IsBusy = false;
                    return;
                }
                IsBusy = false;
            });
        }

        private void InitSwitchButton()
        {
            var buttonManual = new ZgButtonGroupItem { Command = ManualCommand, Label = AppResources.Button_Manual };
            var buttonSync = new ZgButtonGroupItem { Command = SyncCommand, Label = AppResources.Button_Sync };

            GroupItems = new List<ZgButtonGroupItem>
            {
                buttonManual,
                buttonSync
            };

            SelectedGroupItem = buttonManual;
        }

        private async Task CreateSummary(DateTime dateTime)
        {
            Provider.GetActivitySummary(dateTime, _cts.Token).ContinueWith(async result =>
            {
                if (result.Result.Success)
                {
                    Summary = result.Result.Value;
                    SummaryText = string.Format(AppResources.Activities_SummaryText, Summary);

                    var sMins = Summary; // Commented old logic as values are coming in minutes format // Summary * 100;
                    var sTimeSpan = TimeSpan.FromMinutes(sMins);
                    SummaryTextHr = sTimeSpan.Hours.ToString();
                    SummaryTextMin = sTimeSpan.Minutes.ToString();

                    return;
                }

                if (result.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                    result.Result.ErrorReason == ErrorReason.TimeOut)
                    return;

                IsBusy = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.Activities_Title, result.Result.ErrorMessage,
 AppResources.Button_Ok);
                });
            });
        }

        private void CreateExerciseActivityProxy(DateTime dateTime)
        {
            MinDuration = ActivityConstants.MinDayToDayDuration;
            MaxDuration = ActivityConstants.MaxDayToDayDuration;

            ExerciseActivityProxy = new ExerciseActivityProxy
            {
                ExerciseDateTime = dateTime.Date,
                ExerciseTime = dateTime.TimeOfDay,
                Duration = 30,
                SelectedIntensityIndex = 0
            };
        }

        private void CreateIntensitySource()
        {
            var comboBoxItems = new List<ComboBoxItem>
            {
                new ComboBoxItem {Id = 0, Text = AppResources.Activity_Intensity_Low},
                new ComboBoxItem {Id = 1, Text = AppResources.Activity_Intensity_Moderate},
                new ComboBoxItem {Id = 2, Text = AppResources.Activity_Intensity_Vigorous}
            };

            IntensitySource = new ObservableCollection<ComboBoxItem>(comboBoxItems);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }
    }
}