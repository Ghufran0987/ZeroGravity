using System;
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
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.ViewModels
{
    public class FastingDataPageViewModel : VmBase<IFastingDataPage, IFastingDataPageVmProvider, FastingDataPageViewModel>
    {
        private CancellationTokenSource _cts;
        private IEducationalInfoProvider educationalInfoProvider;

        public FastingDataPageViewModel(IVmCommonService service, IFastingDataPageVmProvider provider, IEducationalInfoProvider _educationalInfoProvider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            FastingDataProxy = new FastingDataProxy();
            educationalInfoProvider = _educationalInfoProvider;
            ShowFastingTimePickerCommand = new DelegateCommand(ShowFastingTimePicker);
            SaveFastingDataCommand = new DelegateCommand(SaveFastingData);
            NavigateToFastingSettingCommand = new DelegateCommand(NavigateToFastingSetting);

            MinFastingDuration = FastingConstants.MinFastingDuration;
            MaxFastingDuration = FastingConstants.MaxFastingDuration;
            FastingMainImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Fasting_Main.png");
            WaterCupImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.FastingClock.png");

            AddWaterAmount = new Command(AddWaterAmountExecute);
            RemoveWaterAmount1 = new Command(RemoveWaterAmountExecute);
        }

        public Command AddWaterAmount { get; }
        public Command RemoveWaterAmount1 { get; }
        private ImageSource _fastingMainImageSource;

        public ImageSource FastingMainImageSource
        {
            get => _fastingMainImageSource;
            set => SetProperty(ref _fastingMainImageSource, value);
        }

        private ImageSource _waterCupImageSource;

        public ImageSource WaterCupImageSource
        {
            get => _waterCupImageSource;
            set => SetProperty(ref _waterCupImageSource, value);
        }

        public DelegateCommand SaveFastingDataCommand { get; set; }
        public DelegateCommand ShowFastingTimePickerCommand { get; set; }
        public DelegateCommand NavigateToFastingSettingCommand { get; set; }

        private bool _openTimeSpanPicker;

        public bool OpenTimeSpanPicker
        {
            get => _openTimeSpanPicker;
            set => SetProperty(ref _openTimeSpanPicker, value);
        }

        private double _minFastingDuration;

        public double MinFastingDuration
        {
            get => _minFastingDuration;
            set => SetProperty(ref _minFastingDuration, value);
        }

        private double _maxFastingDuration;

        public double MaxFastingDuration
        {
            get => _maxFastingDuration;
            set => SetProperty(ref _maxFastingDuration, value);
        }

        private FastingDataProxy _fastingDataProxy;

        public FastingDataProxy FastingDataProxy
        {
            get => _fastingDataProxy;
            set => SetProperty(ref _fastingDataProxy, value);
        }

        private double _fastingDuration;

        public double FastingDuration
        {
            get => _fastingDuration;
            set
            {
                SetProperty(ref _fastingDuration, value);

                if (FastingDataProxy != null)
                {
                    if (value == 24)
                    {
                        FastingDataProxy.Duration = 23.99;
                    }
                    else
                        FastingDataProxy.Duration = value;
                }

                CalculateFinishTime();
                SetEndsAtLabelText();
            }
        }

        private TimeSpan _fastingStartTime;

        public TimeSpan FastingStartTime
        {
            get => _fastingStartTime;
            set
            {
                SetProperty(ref _fastingStartTime, value);

                if (FastingDataProxy != null)
                {
                    // ignore seconds
                    FastingDataProxy.StartTime = new TimeSpan(value.Hours, value.Minutes, 0);
                }

                CalculateFinishTime();
                SetEndsAtLabelText();
            }
        }

        private DateTime _fastingEndDateTime;

        public DateTime FastingEndDateTime
        {
            get => _fastingEndDateTime;
            set
            {
                SetProperty(ref _fastingEndDateTime, value);

                if (FastingDataProxy != null)
                {
                    FastingDataProxy.FinishDateTime = value;
                    FastingEndTime = FastingEndDateTime.ToString(DisplayConversionService.GetTimeDisplayFormat());
                }
            }
        }

        private string _fastingEndTime;

        public string FastingEndTime
        {
            get => _fastingEndTime;
            set
            {
                SetProperty(ref _fastingEndTime, value);
            }
        }

        private string _endsAtText;

        public string EndsAtText
        {
            get { return _endsAtText; }
            set { SetProperty(ref _endsAtText, value); }
        }

        private double _fastingProgress;

        public double FastingProgress
        {
            get => _fastingProgress;
            set
            {
                SetProperty(ref _fastingProgress, value);
            }
        }

        private async void RemoveWaterAmountExecute()
        {
            FastingDuration -= 0.5;
            if (FastingDuration < 0)
                FastingDuration = 0;
        }

        private async void AddWaterAmountExecute()
        {
            FastingDuration += 0.5;
            if (FastingDuration > 24)
                FastingDuration = 24;
        }

        private void CalculateFinishTime()
        {
            var tempDate = new DateTime(FastingDataProxy.Created.Year, FastingDataProxy.Created.Month, FastingDataProxy.Created.Day, FastingStartTime.Hours, FastingStartTime.Minutes, FastingStartTime.Seconds);
            var finishTIme = tempDate.AddHours(FastingDuration);
            FastingEndDateTime = new DateTime(finishTIme.Year, finishTIme.Month, finishTIme.Day, finishTIme.Hour, finishTIme.Minute, finishTIme.Second);
        }

        private void SetEndsAtLabelText()
        {
            var startAt = new DateTime(FastingDataProxy.Created.Year, FastingDataProxy.Created.Month, FastingDataProxy.Created.Day, FastingStartTime.Hours, FastingStartTime.Minutes, FastingStartTime.Seconds);
            var finishAt = startAt.AddHours(FastingDuration);

            EndsAtText = finishAt.Day > startAt.Day ? AppResources.FastingData_EndsAtNextDay : AppResources.FastingData_EndsAtSameDay;
        }

        private async void NavigateToFastingSetting()
        {
            await Service.NavigationService.NavigateAsync(ViewName.FastingSettingPage);
        }

        private void ShowFastingTimePicker()
        {
            OpenTimeSpanPicker = !OpenTimeSpanPicker;
        }

        private void CalculateFastingProgress(double duration, DateTime startDateTime)
        {
            var remaintime = DateTime.Now - startDateTime;

            // double remainFasting = (remaintime.Hours + remaintime.Minutes / 100.0 + remaintime.Seconds / 10000.0) * (remaintime > TimeSpan.Zero ? 1 : -1);

            if (remaintime.TotalHours > duration)
            {
                FastingProgress = 100;
            }
            else
            {
                FastingProgress = remaintime.TotalHours * 100 / duration;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<FastingNavParams>(parameters);

            if (navparams != null)
            {
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);

                //var isAuthorized = await ValidateToken();
                //if (isAuthorized) await

                GetFastingDataAsync(navparams.TargetDateTime);
            }
            else
            {
                CreateFastingDataProxy();
            }

            var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.Fasting);
            if (showOverlay)
            {
                IsLoadingImageBusy = true;
                OpenOverlay();
                educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.Fasting).ContinueWith(async apiCallEducation =>
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

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Fasting);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.Fasting);
        }

        private void CreateFastingDataProxy()
        {
            FastingDataProxy = new FastingDataProxy
            {
                Created = DateTime.Now
            };

            FastingDuration = 0;
            FastingStartTime = DateTime.Now.TimeOfDay;
        }

        private async Task GetFastingDataAsync(DateTime targeDateTime)
        {
            IsBusy = true;

            Provider.GetFastingDataByDateAsnyc(targeDateTime, _cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation(
                        $"FastingData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");
                    if (apiCallResult.Result.Value.Duration > 0)
                    {
                        FastingDataProxy = apiCallResult.Result.Value;
                        FastingStartTime = apiCallResult.Result.Value.StartTime;
                        FastingDuration = apiCallResult.Result.Value.Duration == 23.99 ? 24 : apiCallResult.Result.Value.Duration;
                        CalculateFastingProgress(FastingDuration, FastingDataProxy.StartDateTime);
                    }
                    else
                    {
                        CreateFastingDataProxy();
                        FastingProgress = 0;
                    }
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.NoData)
                    {
                    }
                    else if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        // do nothing
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.FastingData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                        });
                    }
                }

                IsBusy = false;
            });
        }

        private async void SaveFastingData()
        {
            if (FastingDataProxy.Duration == 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync("Fasting", "Minimum fasting time shoud be greater than 0", AppResources.Button_Ok);
                });
                return;
            }

            FastingDataProxy.Created = DateTime.Now;

            if (FastingDataProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateFastingData();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateFastingData();
            }
        }

        private async Task CreateFastingData()
        {
            IsBusy = true;

            var apiCallResult = await Provider.CreateFastingDataAsnyc(FastingDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"FastingData for Account: {FastingDataProxy.AccountId} successfully created.");

                FastingDataProxy = apiCallResult.Value;

                CalculateFastingProgress(FastingDataProxy.Duration, FastingDataProxy.StartDateTime);
                IsBusy = false;
                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingData_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
            }
        }

        private async Task UpdateFastingData()
        {
            IsBusy = true;

            var apiCallResult = await Provider.UpdateFastingDataAsync(FastingDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"FastingData for Account: {FastingDataProxy.AccountId} successfully updated.");

                FastingDataProxy = apiCallResult.Value;

                CalculateFastingProgress(FastingDataProxy.Duration, FastingDataProxy.StartDateTime);

                IsBusy = false;
                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingData_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;

                return;
            }

            IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }
    }
}