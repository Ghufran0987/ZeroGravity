using System;
using System.Collections.Generic;
using System.Linq;
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
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace ZeroGravity.Mobile.ViewModels
{
    public class
        IntegrationsPageViewModel : VmBase<IIntegrationsPage, IIntegrationsPageVmProvider, IntegrationsPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private CancellationTokenSource _cts;

        private List<IntegrationDataProxy> _integrationDataProxies;

        private string _searchText;
        private List<IntegrationDataProxy> _storedIntegrationDataProxies;
        private IIntegrationDetailPageVmProvider _provider1;

        public IntegrationsPageViewModel(IVmCommonService service, IIntegrationsPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator, IIntegrationDetailPageVmProvider provider1) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;
            _provider1 = provider1;
            IntegrationDataProxies = new List<IntegrationDataProxy>() {
                new IntegrationDataProxy() {
                    Id = 1,
                    Image = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png"),
                    IntegrationType = 0,
                    Name = "Miboko"
                } ,
                 new IntegrationDataProxy() {
                    Id = 2,
                    Image = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.fitbit.png"),
                    IntegrationType = 1,
                    Name = "Fitbit"
                } ,
                 new IntegrationDataProxy() {
                    Id = 3,
                    Image = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Apple_Watch.png"),
                    IntegrationType = 1,
                    Name = "Apple Watch"
                } ,
                 new IntegrationDataProxy() {
                    Id = 4,
                    Image = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Garmin-Logo.wine.png"),
                    IntegrationType = 1,
                    Name = "Garmin Watch"
                } ,
                 new IntegrationDataProxy() {
                    Id = 5,
                    Image = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Samsung_Galaxy_Gear-Logo.wine.png"),
                    IntegrationType = 1,
                    Name = "Samsung Watch"
                } ,
            };
            //  _storedIntegrationDataProxies = new List<IntegrationDataProxy>();

            SearchIntegrationsCommand = new DelegateCommand(SearchIntegrations);

            OnItemTappedCommand = new Command<object>(OnItemTappedAsync);
        }

        public DelegateCommand SearchIntegrationsCommand { get; set; }
        public Command<object> OnItemTappedCommand { get; set; }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public List<IntegrationDataProxy> IntegrationDataProxies
        {
            get => _integrationDataProxies;
            set => SetProperty(ref _integrationDataProxies, value);
        }

        private async void OnItemTappedAsync(object obj)
        {
            var itemEventArgs = obj as ItemTappedEventArgs;

            if (itemEventArgs?.ItemData is IntegrationDataProxy proxy)
            {
                switch (proxy.IntegrationType)
                {
                    case 0:
                        // Sugar Beat
                        var navigationParams = new PageNavigationParams
                        {
                            DateTime = DateTime.Now,
                            PageName = ViewName.SugarBeatScanPage
                        };

                        _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
                        break;

                    //case 1:
                    //    // Fitbit
                    //    //var fitbitAccount = await GetFitbitAccountAsync();

                    //    //if (fitbitAccount != null)
                    //    //{
                    //    //    _provider1.OpenNativeWebBrowser(fitbitAccount.AuthenticationUrl);
                    //    //}

                    //    // _eventAggregator.GetEvent<IntegrationDetailNavigationEvent>().Publish(proxy);
                    //    break;

                    default:
                        //_eventAggregator.GetEvent<IntegrationDetailNavigationEvent>().Publish(proxy);
                        //Device.BeginInvokeOnMainThread(async () =>
                        //{
                            await Service.DialogService.DisplayAlertAsync("Device Integration", "Comming Soon", AppResources.Button_Ok);
                       // });
                        break;
                }
            }
        }

        private void SearchIntegrations()
        {
            IntegrationDataProxies = string.IsNullOrEmpty(SearchText)
                ? _storedIntegrationDataProxies
                : _storedIntegrationDataProxies.Where(_ => _.Name.Contains(SearchText)).ToList();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            try
            {
                //var isAuthorized = await ValidateToken();
                //if (isAuthorized)

                //  GetIntegrationDatasAsync();
            }
            catch (Exception)
            {
                IntegrationDataProxies = new List<IntegrationDataProxy>();
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private async Task<FitbitAccountProxy> GetFitbitAccountAsync()
        {
            IsBusy = true;

            var apiCallResult =
                await _provider1.GetFitbitAccountAsnyc(_cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $" DayToDayActivity for Account: {apiCallResult.Value.AccountId} successfully loaded.");

                return apiCallResult.Value;
            }

            if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
            {
                IsBusy = false;
                return null;
            }

            await Service.DialogService.DisplayAlertAsync(AppResources.IntegrationDetail_Fitbit_Title,
                apiCallResult.ErrorMessage,
                AppResources.Button_Ok);

            IsBusy = false;
            return null;
        }

        private async void GetIntegrationDatasAsync()
        {
            // IsBusy = true;

            Provider.GetIntegrationDataListAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    IntegrationDataProxies = apiCallResult.Result.Value;
                    _storedIntegrationDataProxies = apiCallResult.Result.Value;
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
                        await Service.DialogService.DisplayAlertAsync(AppResources.AccountOverview_Integrations,
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