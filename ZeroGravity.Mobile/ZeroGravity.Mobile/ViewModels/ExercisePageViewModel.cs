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
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ExercisePageViewModel : VmBase<IExercisePage, IExercisePageVmProvider, ExercisePageViewModel>
    {
        private ExerciseActivityProxy _exerciseActivityProxy;

        private double _maxDuration;

        private double _minDuration;

        private bool _openDateTimePicker;

        private bool _openTimeSpanPicker;

        private CancellationTokenSource _cts;

        public ExercisePageViewModel(IVmCommonService service, IExercisePageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            ShowExerciseTimePickerCommand = new DelegateCommand(ShowExerciseTimePicker);
            ShowExerciseDatePickerCommand = new DelegateCommand(ShowExerciseDatePicker);

            SaveExerciseDataCommand = new DelegateCommand(SaveExerciseActivity);

            ExerciseActivityProxy = new ExerciseActivityProxy();

            MinDuration = ActivityConstants.MinExerciseDuration;
            MaxDuration = ActivityConstants.MaxExerciseDuration;

            ExerciseList = Provider.GetExerciseItems();

            CustomLabelsCollection = new ObservableCollection<Items>();
            CustomLabelsCollection.Add(new Items { Value = ExerciseActivityProxy.DailyThreshold, Label = "\u25B3" });
        }

        private ObservableCollection<Items> _customLabelsCollection;

        public ObservableCollection<Items> CustomLabelsCollection
        {
            get => _customLabelsCollection;
            set => SetProperty(ref _customLabelsCollection, value);
        }

        public ObservableCollection<ComboBoxItem> ExerciseList { get; set; }

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

        public ExerciseActivityProxy ExerciseActivityProxy
        {
            get => _exerciseActivityProxy;
            set => SetProperty(ref _exerciseActivityProxy, value);
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

        public DelegateCommand ShowExerciseTimePickerCommand { get; set; }
        public DelegateCommand ShowExerciseDatePickerCommand { get; set; }
        public DelegateCommand SaveExerciseDataCommand { get; set; }

        private void ShowExerciseDatePicker()
        {
            OpenDateTimePicker = !OpenDateTimePicker;
        }

        private void ShowExerciseTimePicker()
        {
            OpenTimeSpanPicker = !OpenTimeSpanPicker;
        }

        private async void SaveExerciseActivity()
        {
            if (ExerciseActivityProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateExerciseActivity();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateExerciseActivity();
            }
        }

        private async Task UpdateExerciseActivity()
        {
            IsBusy = true;

            var apiCallResult = await Provider.UpdateExerciseActivitysnyc(ExerciseActivityProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"ExerciseActivity for Account: {ExerciseActivityProxy.AccountId} successfully updated.");

                ExerciseActivityProxy = apiCallResult.Value;

                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.Exercise_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<ActivityNavParams>(parameters);

            if (navparams != null)
            {
                //var isAuthorized = await ValidateToken();
                //if (isAuthorized)

                GetExerciseActivityAsync(navparams.ActivityId);

                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);
            }
            else
            {
                var currentDateTime = DateTime.Now;
                ExerciseActivityProxy = new ExerciseActivityProxy
                {
                    ExerciseDateTime = currentDateTime,
                    ExerciseTime = new TimeSpan(currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second),
                    Duration = 60
                };
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private async Task CreateExerciseActivity()
        {
            IsBusy = true;

            var apiCallResult = await Provider.CreateExerciseActivityAsnyc(ExerciseActivityProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"ExerciseActivity for Account: {ExerciseActivityProxy.AccountId} successfully created.");

                ExerciseActivityProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.Exercise_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async void GetExerciseActivityAsync(int activityId)
        {
            //
            //IsBusy = true;

            //var apiCallResult =
            //    await

            Provider.GetExerciseActivityAsnyc(activityId, _cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation(
                        $"ExerciseActivity for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    ExerciseActivityProxy = apiCallResult.Result.Value;
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsBusy = false;
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.Exercise_Title,
                    apiCallResult.Result.ErrorMessage,
                    AppResources.Button_Ok);
                    });

                    IsBusy = false;
                    return;
                }

                IsBusy = false;
            });
        }
    }
}