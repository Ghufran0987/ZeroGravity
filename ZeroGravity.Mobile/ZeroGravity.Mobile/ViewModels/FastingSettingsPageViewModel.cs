using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
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

namespace ZeroGravity.Mobile.ViewModels
{
    public class FastingSettingsPageViewModel : VmBase<IFastingSettingsPage, IFastingSettingsPageVmProvider,
        FastingSettingsPageViewModel>
    {
        private FastingSettingProxy _fastingSettingProxy;

        private CancellationTokenSource _cts;

        public FastingSettingsPageViewModel(IVmCommonService service, IFastingSettingsPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveFastingSettingCommand = new DelegateCommand(SaveFastingSetting);
        }

        public FastingSettingProxy FastingSettingProxy
        {
            get => _fastingSettingProxy;
            set => SetProperty(ref _fastingSettingProxy, value);
        }

        public DelegateCommand SaveFastingSettingCommand { get; set; }

        private async void SaveFastingSetting()
        {
            if (FastingSettingProxy.Id != 0)
            {
                await UpdateFastingSetting();
            }
            else
            {
                await CreateFastingData();
            }
        }

        private async Task UpdateFastingSetting()
        {
            IsBusy = true;

            var apiCallResult = await Provider.UpdateFastingSettingAsync(FastingSettingProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"FastingSetting for Account: {FastingSettingProxy.AccountId} successfully updated.");

                FastingSettingProxy = apiCallResult.Value;

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingSetting_Title,
                    AppResources.Common_SaveSuccessful,
                    AppResources.Button_Ok);
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingSetting_Title,
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

            ////if (await ValidateToken())
            ////{
            ////    await

            GetFastingSettingAsync();
            // }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private async Task CreateFastingData()
        {
            IsBusy = true;

            var apiCallResult = await Provider.CreateFastingDataAsnyc(FastingSettingProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"FastingSetting for Account: {FastingSettingProxy.AccountId} successfully created.");

                FastingSettingProxy = apiCallResult.Value;

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingSetting_Title,
                    AppResources.Common_SaveSuccessful,
                    AppResources.Button_Ok);
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.FastingSetting_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task GetFastingSettingAsync()
        {
            //  IsBusy = true;

            //var apiCallResult =
            //    await
            Provider.GetFastingSettingAsync(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation(
                        $"FastingSetting for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    FastingSettingProxy = apiCallResult.Result.Value;
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
                        await Service.DialogService.DisplayAlertAsync(AppResources.FastingSetting_Title,
                        apiCallResult.Result.ErrorMessage,
                        AppResources.Button_Ok);
                    });
                }

                IsBusy = false;
            });
        }
    }
}