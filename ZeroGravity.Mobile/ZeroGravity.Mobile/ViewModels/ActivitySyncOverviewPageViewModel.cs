using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
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
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ActivitySyncOverviewPageViewModel : VmBase<IActivitySyncOverviewPage,
        IActivitySyncOverviewPageVmProvider, ActivitySyncOverviewPageViewModel>
    {
        private ActivityNavParams _activityNavParams;

        public Command<object> OnItemTappedCommand { get; set; }

        private CancellationTokenSource _cts;

        public ActivitySyncOverviewPageViewModel(IVmCommonService service, IActivitySyncOverviewPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            OnItemTappedCommand = new Command<object>(OnItemTapped);
        }

        public DateTime TargetDateTime { get; set; }

        private void OnItemTapped(object obj)
        {
            var itemEventArgs = obj as ItemTappedEventArgs;

            if (itemEventArgs?.ItemData is IntegrationDataProxy proxy)
            {
                GetActivityDataAsync(proxy, TargetDateTime);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<ActivityNavParams>(parameters);

            if (navparams != null)
            {
                _activityNavParams = navparams;

                var isAuthorized = await ValidateToken();
                if (isAuthorized) GetLinkedIntegrationsAsync();

                TargetDateTime = navparams.TargetDateTime;
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);
            }
            else if (_activityNavParams != null)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) GetLinkedIntegrationsAsync();

                TargetDateTime = _activityNavParams.TargetDateTime;
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_activityNavParams.TargetDateTime);
            }
            else
            {
                IntegrationDataProxies = new List<IntegrationDataProxy>();
            }
        }

        private List<IntegrationDataProxy> _integrationDataProxies;

        public List<IntegrationDataProxy> IntegrationDataProxies
        {
            get => _integrationDataProxies;
            set => SetProperty(ref _integrationDataProxies, value);
        }


        private async void GetActivityDataAsync(IntegrationDataProxy integrationDataProxy, DateTime targetDate)
        {
            IsBusy = true;

            var apiCallResult =
                await Provider.GetActivitySyncDataAsnyc(integrationDataProxy, targetDate, _cts.Token);

            if (apiCallResult.Success)
            {
                var syncActivityProxies = apiCallResult.Value;

                var navParams = NavigationParametersHelper.CreateNavigationParameter(new SyncActivityNavParams
                    {TargetDateTime = targetDate, ExerciseActivityProxies = syncActivityProxies});

                await Service.NavigationService.NavigateAsync(ViewName.ActivitySyncDetailPage, navParams);
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.ActivitySyncOverview_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async void GetLinkedIntegrationsAsync()
        {
            IsBusy = true;

            var apiCallResult =
                await Provider.GetLinkedIntegrationListAsnyc(_cts.Token);

            if (apiCallResult.Success)
            {
                IntegrationDataProxies = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.ActivitySyncOverview_Title,
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