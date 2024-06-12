using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class
        StepCountPageViewModel : VmBase<IStepCountPage, IStepCountPageVmProvider, StepCountPageViewModel>
    {
        private CancellationTokenSource _cts;
        private DateTime _actualDateTime;

        private int _stepCount;

        private StepCountDataProxy _stepCountDataProxy;

        public StepCountPageViewModel(IVmCommonService service, IStepCountPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveStepCountDataCommand = new DelegateCommand(SaveStepCountData);

            StepCountDataProxy = new StepCountDataProxy
            {
                TargetDate = DateTime.Today,
                StepCount = StepCount
            };
        }

        public DelegateCommand SaveStepCountDataCommand { get; set; }

        public int StepCount
        {
            get => _stepCount;
            set => SetProperty(ref _stepCount, value);
        }

        public StepCountDataProxy StepCountDataProxy
        {
            get => _stepCountDataProxy;
            set => SetProperty(ref _stepCountDataProxy, value);
        }

        private void GetCurrentStepCount()
        {
            StepCount = Provider.GetCurrentSteps();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            IsBusy = true;

            try
            {
                var navParamsTemp = NavigationParametersHelper.GetNavigationParameters<MetabolicHealthNavParams>(parameters);
                if (navParamsTemp == null)
                {
                    _actualDateTime = DateTime.Now;
                }
                else
                {
                    _actualDateTime = navParamsTemp.TargetDateTime.Date;
                    _actualDateTime = _actualDateTime.Add(DateTime.Now.TimeOfDay);
                }

                

                if (!Provider.IsSensorAvailable())
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.StepCount_Title,
                        AppResources.StepCount_NotAvailable,
                        AppResources.Button_Ok);

                    await Service.NavigationService.GoBackAsync();
                }
                else
                {
                    Provider.InitSensorService();
                    var navparams = NavigationParametersHelper.GetNavigationParameters<ActivityNavParams>(parameters);

                    if (navparams != null)
                    {
                        Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);

                        var isAuthorized = await ValidateToken();
                        if (isAuthorized) GetStepCountDataAsync(navparams.TargetDateTime);
                    }
                    else
                    {
                        var currentDateTime = DateTime.Today;
                        StepCountDataProxy = new StepCountDataProxy
                        {
                            TargetDate = currentDateTime,
                            StepCount = StepCount
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error when {nameof(OnNavigatedTo)} {nameof(StepCountPageViewModel)}.\n{e}\n{e.Message}");
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }


        }

        private async void SaveStepCountData()
        {
            if (StepCountDataProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateStepCountData();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateStepCountData();
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            Provider.StopSensorService();

            CancelPendingRequest(_cts);
        }


        private async Task UpdateStepCountData()
        {
            IsBusy = true;

            StepCount = Provider.GetCurrentSteps();
            StepCountDataProxy.StepCount = StepCount;

            StepCountDataProxy.TargetDate = _actualDateTime;
            var apiCallResult = await Provider.UpdateStepCountDataAsync(StepCountDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"StepCountData for Account: {StepCountDataProxy.AccountId} successfully updated.");

                StepCountDataProxy = apiCallResult.Value;

                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.StepCount_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task CreateStepCountData()
        {
            IsBusy = true;

            StepCountDataProxy.TargetDate = _actualDateTime;
            var apiCallResult = await Provider.CreateStepCountDataAsync(StepCountDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"StepCountData for Account: {StepCountDataProxy.AccountId} successfully created.");

                StepCountDataProxy = apiCallResult.Value;

                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.StepCount_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async void GetStepCountDataAsync(DateTime targetDateTime)
        {
            IsBusy = true;

            var apiCallResult =
                await Provider.GetStepCountDataByDateAsync(targetDateTime, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $" StepCountData for Account: {apiCallResult.Value.AccountId} successfully loaded.");

                StepCountDataProxy = apiCallResult.Value;

                GetCurrentStepCount();

                if(StepCountDataProxy.StepCount < StepCount)
                {
                    StepCountDataProxy.StepCount = StepCount;
                }
            }
            else
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.StepCount_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }
    }
}