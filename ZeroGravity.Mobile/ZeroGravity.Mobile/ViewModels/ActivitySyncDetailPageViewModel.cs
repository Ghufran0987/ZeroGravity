using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ActivitySyncDetailPageViewModel : VmBase<IActivitySyncDetailPage, IActivitySyncDetailPageVmProvider,
        ActivitySyncDetailPageViewModel>
    {
        private CancellationTokenSource _cts;

        private List<SyncActivityProxy> _syncActivityProxies;

  

        public List<SyncActivityProxy> SyncActivityProxies
        {
            get => _syncActivityProxies;
            set => SetProperty(ref _syncActivityProxies, value);
        }


        public ActivitySyncDetailPageViewModel(IVmCommonService service, IActivitySyncDetailPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveCommand = new DelegateCommand(SaveSyncActivities);


        }

        private async void SaveSyncActivities()
        {
            var isAuthorized = await ValidateToken();

            if (isAuthorized)
            {
                var selectedActivities = SyncActivityProxies.Where(_ => _.IsSelectedForSync).ToList();

                if(!selectedActivities.Any())
                    return;

                IsBusy = true;

                var apiCallResult =
                    await Provider.SynchroniseActivitiesAsnyc(selectedActivities, _cts.Token);

                if (apiCallResult.Success)
                {
                    await Service.NavigationService.GoBackAsync();
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
            }


            IsBusy = false;
        }

        public DelegateCommand SaveCommand { get; set; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<SyncActivityNavParams>(parameters);

            if (navparams != null)
            {

                SyncActivityProxies = new List<SyncActivityProxy>(navparams.ExerciseActivityProxies);

                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.TargetDateTime);
            }
            else
            {
                SyncActivityProxies = new List<SyncActivityProxy>();
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }




    }
}