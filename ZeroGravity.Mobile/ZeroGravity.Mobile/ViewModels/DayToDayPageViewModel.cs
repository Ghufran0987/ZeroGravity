using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.SfRangeSlider.XForms;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.ViewModels
{
    public class DayToDayPageViewModel : VmBase<IDayToDayPage, IDayToDayPageVmProvider, DayToDayPageViewModel>
    {
        private ObservableCollection<Items> _customLabelsCollection;

        private DayToDayActivityProxy _dayToDayActivityProxy;

        private double _maxDuration;
        private double _minDuration;

        private bool _openDateTimePicker;

        private bool _openTimeSpanPicker;

        private CancellationTokenSource _cts;

        public DayToDayPageViewModel(IVmCommonService service, IDayToDayPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            ShowDayToDayTimePickerCommand = new DelegateCommand(ShowDayToDayTimePicker);
            ShowDayToDayDatePickerCommand = new DelegateCommand(ShowDayToDayDatePicker);

            SaveDayToDayActivityCommand = new DelegateCommand(SaveDayToDayActivity);

            DayToDayActivityProxy = new DayToDayActivityProxy();

            MinDuration = ActivityConstants.MinDayToDayDuration;
            MaxDuration = ActivityConstants.MaxDayToDayDuration;

            CustomLabelsCollection = new ObservableCollection<Items>();
            CustomLabelsCollection.Add(new Items { Value = DayToDayActivityProxy.DailyThreshold, Label = "\u25B3" });
        }

        public ObservableCollection<Items> CustomLabelsCollection
        {
            get => _customLabelsCollection;
            set => SetProperty(ref _customLabelsCollection, value);
        }

        public bool OpenTimeSpanPicker
        {
            get => _openTimeSpanPicker;
            set => SetProperty(ref _openTimeSpanPicker, value);
        }

        public bool OpenDateTimePicker
        {
            get => _openDateTimePicker;
            set => SetProperty(ref _openDateTimePicker, value);
        }

        public DayToDayActivityProxy DayToDayActivityProxy
        {
            get => _dayToDayActivityProxy;
            set => SetProperty(ref _dayToDayActivityProxy, value);
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

        public DelegateCommand ShowDayToDayTimePickerCommand { get; set; }
        public DelegateCommand ShowDayToDayDatePickerCommand { get; set; }
        public DelegateCommand SaveDayToDayActivityCommand { get; set; }

        private void ShowDayToDayDatePicker()
        {
            OpenDateTimePicker = !OpenDateTimePicker;
        }

        private void ShowDayToDayTimePicker()
        {
            OpenTimeSpanPicker = !OpenTimeSpanPicker;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<ActivityNavParams>(parameters);

            if (navparams != null)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) GetDayToDayActivityAsync(navparams.ActivityId);

                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);
            }
            else
            {
                var currentDateTime = DateTime.Now;
                DayToDayActivityProxy = new DayToDayActivityProxy
                {
                    DayToDayDateTime = currentDateTime,
                    DayToDayTime = new TimeSpan(currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second),
                    Duration = 60
                };
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private async void SaveDayToDayActivity()
        {
            if (DayToDayActivityProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateDayToDayActivity();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateDayToDayActivity();
            }
        }

        private async Task UpdateDayToDayActivity()
        {
            IsBusy = true;

            var apiCallResult = await Provider.UpdateDayToDayActivityAsnyc(DayToDayActivityProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"DayToDayActivity for Account: {DayToDayActivityProxy.AccountId} successfully updated.");

                DayToDayActivityProxy = apiCallResult.Value;

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

                await Service.DialogService.DisplayAlertAsync(AppResources.DayToDay_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task CreateDayToDayActivity()
        {
            IsBusy = true;

            var apiCallResult = await Provider.CreatDayToDayActivityAsnyc(DayToDayActivityProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"DayToDayActivity for Account: {DayToDayActivityProxy.AccountId} successfully created.");

                DayToDayActivityProxy = apiCallResult.Value;

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

                await Service.DialogService.DisplayAlertAsync(AppResources.DayToDay_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async void GetDayToDayActivityAsync(int activityId)
        {
            IsBusy = true;

            var apiCallResult =
                await Provider.GetDayToDayActivityAsnyc(activityId, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $" DayToDayActivity for Account: {apiCallResult.Value.AccountId} successfully loaded.");

                DayToDayActivityProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.DayToDay_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }
    }
}