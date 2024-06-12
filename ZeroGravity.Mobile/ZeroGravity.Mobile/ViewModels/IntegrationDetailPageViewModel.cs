using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
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
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class IntegrationDetailPageViewModel : VmBase<IIntegrationDetailPage, IIntegrationDetailPageVmProvider,
        IntegrationDetailPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private CancellationTokenSource _cts;

        private IntegrationDataProxy _integrationDataProxy;

        public IntegrationDataProxy IntegrationDataProxy
        {
            get => _integrationDataProxy;
            set => SetProperty(ref _integrationDataProxy, value);
        }

        private string _integrationTitle;

        public string IntegrationTitle
        {
            get => _integrationTitle;
            set => SetProperty(ref _integrationTitle, value);
        }

        private string _integrationTypeName;

        public string IntegrationTypeName
        {
            get => _integrationTypeName;
            set => SetProperty(ref _integrationTypeName, value);
        }

        private string _integrationDescription;

        public string IntegrationDescription
        {
            get => _integrationDescription;
            set => SetProperty(ref _integrationDescription, value);
        }

        private string _integrationButtonText;

        public string IntegrationButtonText
        {
            get => _integrationButtonText;
            set => SetProperty(ref _integrationButtonText, value);
        }

        public DelegateCommand LinkIntegrationCommand { get; set; }

        public IntegrationDetailPageViewModel(IVmCommonService service, IIntegrationDetailPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;
            IntegrationDataProxy = new IntegrationDataProxy();

            LinkIntegrationCommand = new DelegateCommand(LinkIntegration);

            PlaceholderImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.glucose.png");
        }

        private bool _showPlaceholder;

        public bool ShowPlaceholder
        {
            get => _showPlaceholder;
            set => SetProperty(ref _showPlaceholder, value);
        }

        private ImageSource _placeholderImageSource;

        public ImageSource PlaceholderImageSource
        {
            get => _placeholderImageSource;
            set => SetProperty(ref _placeholderImageSource, value);
        }

        private async void LinkIntegration()
        {
            if (IntegrationDataProxy != null)
            {
                if (IntegrationDataProxy.Name.Equals(IntegrationNameConstants.SugarBeat))
                {
                    //todo integrate sugarbeat logic
                }
                else if (IntegrationDataProxy.Name.Equals(IntegrationNameConstants.Fitbit))
                {
                    //todo integrate Fitbit logic

                    var fitbitAccount = await GetFitbitAccountAsync();

                    if (fitbitAccount != null)
                    {
                         Provider.OpenNativeWebBrowser(fitbitAccount.AuthenticationUrl);
                    }
                }
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<IntegrationNavParam>(parameters);

            if (navparams != null)
            {
                //var isAuthorized = await ValidateToken();
                //if (isAuthorized) 
                    GetIntegrationDetails(navparams.IntegrationDataProxy);
            }
            else
            {
                await Service.NavigationService.GoBackAsync();
            }
        }

        private void GetIntegrationDetails(IntegrationDataProxy navparamsIntegrationDataProxy)
        {
            if (navparamsIntegrationDataProxy != null)
            {
                IntegrationDataProxy = navparamsIntegrationDataProxy;

                switch ((IntegrationType)IntegrationDataProxy.IntegrationType)
                {
                    case IntegrationType.Device:

                        if (IntegrationDataProxy.Name.Equals(IntegrationNameConstants.SugarBeat))
                        {
                            IntegrationTitle = AppResources.IntegrationDetail_Sugarbeat_Title;
                            IntegrationTypeName = AppResources.IntegrationDetail_Device;
                            IntegrationDescription = AppResources.IntegrationDetail_Sugarbeat_Description;

                            ShowPlaceholder = true;
                        }

                        //IntegrationButtonText = !IntegrationDataProxy.IsLinked ? AppResources.IntegrationDetail_Link_Device : AppResources.IntegrationDetail_Unlink_Device;
                        IntegrationButtonText =  AppResources.IntegrationDetail_Link_Device ;

                        break;
                    case IntegrationType.Service:

                        if (IntegrationDataProxy.Name.Equals(IntegrationNameConstants.Fitbit))
                        {
                            IntegrationTitle = AppResources.IntegrationDetail_Fitbit_Title;
                            IntegrationTypeName = AppResources.IntegrationDetail_Device;
                            IntegrationDescription = AppResources.IntegrationDetail_Fitbit_Description;

                            ShowPlaceholder = false;
                        }

                        //IntegrationButtonText = !IntegrationDataProxy.IsLinked ? AppResources.IntegrationDetail_Link_Service : AppResources.IntegrationDetail_Link_Service;
                        IntegrationButtonText =  AppResources.IntegrationDetail_Link_Device ;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
                await Provider.GetFitbitAccountAsnyc(_cts.Token);

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
    }
}