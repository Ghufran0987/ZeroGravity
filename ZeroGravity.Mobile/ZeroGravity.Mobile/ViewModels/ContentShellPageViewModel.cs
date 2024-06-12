using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ContentShellPageViewModel : VmBase<IContentShellPage, IContentShellPageVmProvider,
        ContentShellPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private string _currentViewName;

        public ContentShellPageViewModel(IVmCommonService service, IContentShellPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator, ISecureStorageService secureStorageService) : base(service, provider, loggerFactory)
        {
            _secureStorageService = secureStorageService;
            _eventAggregator = eventAggregator;
            RegisterForTabViewEvents();

            Connectivity.ConnectivityChanged -= ConnectivityChanged;

            _hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;

            Connectivity.ConnectivityChanged += ConnectivityChanged;

            if (!HasInternetConnection)
            {
                Service.EventAggregator.GetEvent<NoInternetConnectionEvent>().Publish();
            }
            _eventAggregator.GetEvent<AppResumedEvent>().Subscribe(OnAppResumed);
            _eventAggregator?.GetEvent<SugarBeatShowAlertEvent>().Subscribe(OnSugarBeatAlert);
            _eventAggregator?.GetEvent<SugarBeatDeviceLostAlertEvent>().Subscribe(OnSugarBeatDeviceLostAlert);

            
            this.ValidateToken();
        }

        private void OnSugarBeatDeviceLostAlert(string alert)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.SugarBeat_ALert_Title, alert, AppResources.Button_Ok);
              if (_currentViewName== ViewName.MetabolicHealthPage
                || _currentViewName == ViewName.SugarBeatConnectPage
                ||_currentViewName == ViewName.SugarBeatMainPage)
                {
                    await Service.NavigationService.GoBackToRootAsync();
                }
            });
        }

        private void OnSugarBeatAlert(string alert)
        {
            //Display the Alert to the user
            //   MessageBox.show
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.SugarBeat_ALert_Title, alert, AppResources.Button_Ok);
            });
        }

        private async void OnAppResumed()
        {
            try
            {
                await this.ValidateToken();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message);
            }
        }

        private bool _hasInternetConnection;

        public bool HasInternetConnection
        {
            get { return _hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet; }
            set { SetProperty(ref _hasInternetConnection, value); }
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            switch (e.NetworkAccess)
            {
                case NetworkAccess.Unknown:
                    // Unable to determine internet connectivity.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.None:
                    // No connectivity is available.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.Local:
                    HasInternetConnection = false;
                    // Local network access only.
                    break;

                case NetworkAccess.ConstrainedInternet:
                    // Limited internet access.
                    // Indicates captive portal connectivity, where local access to a web portal is provided,
                    // but access to the Internet requires that specific credentials are provided via a portal.
                    HasInternetConnection = false;
                    break;

                case NetworkAccess.Internet:
                    // Local and internet access
                    HasInternetConnection = true;
                    break;
            }

            if (!HasInternetConnection)
            {
                Service.EventAggregator.GetEvent<NoInternetConnectionEvent>().Publish();
            }
        }

        private void RegisterForTabViewEvents()
        {
            DeRegisterForTabViewEvents();

            _eventAggregator.GetEvent<NoInternetConnectionEvent>().Subscribe(OnNoInternetConnectionEvent);
            _eventAggregator.GetEvent<ProfilePageEvent>().Subscribe(OnProfilePageEvent);
            _eventAggregator.GetEvent<NotificationPageEvent>().Subscribe(OnNotificationPageEvent);

            _eventAggregator.GetEvent<AccountDeletePageEvent>().Subscribe(OnAccountDeletePageEvent);
            _eventAggregator.GetEvent<ChangeEmailPageEvent>().Subscribe(OnChangeEmailPageEvent);
            _eventAggregator.GetEvent<ChangePasswordPageEvent>().Subscribe(OnChangePasswordPageEvent);

            _eventAggregator.GetEvent<MealsSnacksNavigationEvent>().Subscribe(OnMealsSnacksNavigation);
            _eventAggregator.GetEvent<IntegrationDetailNavigationEvent>().Subscribe(OnIntegrationDetailNavigation);
            _eventAggregator.GetEvent<PageNavigationEvent>().Subscribe(OnPageNavigationEvent);
            _eventAggregator.GetEvent<ModalPageNavigationEvent>().Subscribe(OnModalPageNavigationEvent);

            _eventAggregator.GetEvent<RefreshTokenEvent>().Subscribe(OnRefreshTokenEvent);
            _eventAggregator.GetEvent<UnauthorizedEvent>().Subscribe(OnUnauthorizedEvent);

            _eventAggregator.GetEvent<SelectTabEvent>().Subscribe(OnSelectTab);
            _eventAggregator.GetEvent<TermsAndPrivacyPageEvent>().Subscribe(OnTermsAndPrivacyPageEvent);
            _eventAggregator.GetEvent<MeditataionDetailEvent>().Subscribe(OnMeditataionDetailEvent);

            Console.WriteLine("=== REGISTERED FOR EVENTS ===");
        }

        private async void OnMeditataionDetailEvent(MeditataionDetailEvent payLoad)
        {
            var navParams = NavigationParametersHelper.CreateNavigationParameter(payLoad);
            await Service.NavigationService.NavigateAsync(payLoad.PageName, navParams);
        }

        private async void OnTermsAndPrivacyPageEvent(TermsAndPrivacyNavParams payload)
        {
            var navParams = NavigationParametersHelper.CreateNavigationParameter(payload);
            await Service.NavigationService.NavigateAsync(payload.PageName, navParams);
        }

        private async void OnModalPageNavigationEvent(PageNavigationParams pageNavigationParams)
        {
            _currentViewName = pageNavigationParams.PageName;

            if (pageNavigationParams.PageName == ViewName.MealsSnacksPage)
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(pageNavigationParams);

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams, true);
            }
        }

        private async void OnIntegrationDetailNavigation(IntegrationDataProxy integrationDataProxy)
        {
            _currentViewName = ViewName.IntegrationDetailPage;

            var navParams = NavigationParametersHelper.CreateNavigationParameter(new IntegrationNavParam
            { IntegrationDataProxy = integrationDataProxy });

            if (integrationDataProxy.Name == IntegrationNameConstants.SugarBeat)
            {
                //TODO Check this navigation now need to send to scan/connect page
                await Service.NavigationService.NavigateAsync(ViewName.SugarBeatConnectPage); // placeholder page
                //await Service.NavigationService.NavigateAsync(ViewName.SugarBeatConnectPage, navParams);
            }
            else if (integrationDataProxy.Name == IntegrationNameConstants.Fitbit)
            {
                await Service.NavigationService.NavigateAsync(ViewName.IntegrationDetailPage, navParams);
            }
            else
            {
                Logger.LogWarning("Unknown Integration type.");
            }
        }

        public static bool IsSessionTimeoutShown = false;

        private async void OnRefreshTokenEvent()
        {
            try
            {
                await Provider.RefreshTokenAsync();
            }
            catch (HttpException ex)
            {
                if (ex.CustomErrorMessage == "Invalid token") // ToDo: use static string
                {
                    var path = Service.NavigationService.GetNavigationUriPath();
                    if (!string.Equals(path, ("/" + ViewName.Login)))
                    {
                        if (await Provider.IsJwtExpiredOrInvalid())
                        {
                            if (IsSessionTimeoutShown == false)
                            {
                                IsSessionTimeoutShown = true;
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    try
                                    {
                                        await Service.DialogService.DisplayAlertAsync(
                                                                       AppResources.SessionTimedOut_Title,
                                                                       AppResources.SessionTimedOut_Msg,
                                                                       AppResources.Button_Ok);

                                        await Service.NavigationService.NavigateAsync("/" + ViewName.Login);
                                    }
                                    catch (Exception e)
                                    {
                                        Logger.LogError(e, e.Message);
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }
        }

        private async void OnNoInternetConnectionEvent()
        {
            await NavigateToNoInternetConnectionPage();
        }

        private async void OnChangePasswordPageEvent()
        {
            _currentViewName = ViewName.ChangePasswordPage;

            await Service.NavigationService.NavigateAsync(ViewName.ChangePasswordPage);
        }

        private async void OnChangeEmailPageEvent()
        {
            _currentViewName = ViewName.ChangeEmailPage;

            await Service.NavigationService.NavigateAsync(ViewName.ChangeEmailPage);
        }

        private async void OnPageNavigationEvent(PageNavigationParams pageNavigationParams)
        {
            _currentViewName = pageNavigationParams.PageName;

            if (pageNavigationParams.PageName == ViewName.MetabolicHealthPage)
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new MetabolicHealthNavParams
                { TargetDateTime = pageNavigationParams.DateTime });

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);

                return;
            }

            if (pageNavigationParams.PageName == ViewName.SugarBeatConnectPage)
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(pageNavigationParams);

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
                return;
            }

            if (pageNavigationParams.PageName.Equals(ViewName.FastingDataPage))
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new FastingNavParams
                { TargetDateTime = pageNavigationParams.DateTime });

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
            }
            else if (pageNavigationParams.PageName.Equals(ViewName.WaterIntakePage) ||
                     pageNavigationParams.PageName.Equals(ViewName.CalorieDrinksAlcoholPage))
            {
                var navigationParameters =
                    NavigationParametersHelper.CreateNavigationParameter(
                        new LiquidIntakeNavParams(pageNavigationParams.DateTime));

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navigationParameters);
            }
            else if (pageNavigationParams.PageName.Equals(ViewName.ActivitiesPage))
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new ActivityNavParams
                { TargetDateTime = pageNavigationParams.DateTime });

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
            }
            else if (pageNavigationParams.PageName.Equals(ViewName.MealsSnacksPage))
            {
                var navParams =
                    NavigationParametersHelper.CreateNavigationParameter(
                        new MealNavParams(pageNavigationParams.DateTime));

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
            }
            else if (pageNavigationParams.PageName.Equals(ViewName.WellbeingDataPage))
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new WellbeingNavParams
                { TargetDateTime = pageNavigationParams.DateTime });

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
            }
            else
            {
                var navParams = NavigationParametersHelper.CreateNavigationParameter(pageNavigationParams);
                Debug.WriteLine("Navigating to " + pageNavigationParams.PageName);

                await Service.NavigationService.NavigateAsync(pageNavigationParams.PageName, navParams);
            }
        }

        private void OnMealsSnacksNavigation()
        {
            _currentViewName = ViewName.MealsSnacksPage;

            Service.NavigationService.NavigateAsync(ViewName.MealsSnacksPage);
        }

        private async void OnAccountDeletePageEvent()
        {
            _currentViewName = ViewName.AccountDeletePage;

            await Service.NavigationService.NavigateAsync(ViewName.AccountDeletePage);
        }

        private async void OnNotificationPageEvent()
        {
            _currentViewName = ViewName.NotificationsPage;
            await Service.NavigationService.NavigateAsync(ViewName.NotificationsPage);
        }

        private async void OnUnauthorizedEvent()
        {
            try
            {
                await Service.DialogService.DisplayAlertAsync(
                    AppResources.SessionTimedOut_Title,
                    AppResources.SessionTimedOut_Msg,
                    AppResources.Button_Ok);

                Page.NavigateFromLastTabContent();

                await Service.NavigationService.NavigateAsync("/" + ViewName.Login);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void OnProfilePageEvent()
        {
            await Service.NavigationService.NavigateAsync(ViewName.ProfilePage);
        }

        private void DeRegisterForTabViewEvents()
        {
            if (_eventAggregator != null)
            {
                _eventAggregator.GetEvent<NoInternetConnectionEvent>().Unsubscribe(OnNoInternetConnectionEvent);
                _eventAggregator.GetEvent<ProfilePageEvent>().Unsubscribe(OnProfilePageEvent);
                _eventAggregator.GetEvent<NotificationPageEvent>().Unsubscribe(OnNotificationPageEvent);
                _eventAggregator.GetEvent<MealsSnacksNavigationEvent>().Unsubscribe(OnMealsSnacksNavigation);
                _eventAggregator.GetEvent<IntegrationDetailNavigationEvent>()
                    .Unsubscribe(OnIntegrationDetailNavigation);
                _eventAggregator.GetEvent<PageNavigationEvent>().Unsubscribe(OnPageNavigationEvent);
                _eventAggregator.GetEvent<ModalPageNavigationEvent>().Unsubscribe(OnModalPageNavigationEvent);
                _eventAggregator.GetEvent<AccountDeletePageEvent>().Unsubscribe(OnAccountDeletePageEvent);
                _eventAggregator.GetEvent<ChangeEmailPageEvent>().Unsubscribe(OnChangeEmailPageEvent);
                _eventAggregator.GetEvent<ChangePasswordPageEvent>().Unsubscribe(OnChangePasswordPageEvent);
                _eventAggregator.GetEvent<RefreshTokenEvent>().Unsubscribe(OnRefreshTokenEvent);
                _eventAggregator.GetEvent<SelectTabEvent>().Unsubscribe(OnSelectTab);
                _eventAggregator.GetEvent<TermsAndPrivacyPageEvent>().Unsubscribe(OnTermsAndPrivacyPageEvent);
                Console.WriteLine("=== DEREGISTERED FROM EVENTS ===");
            }
        }

        private async Task NavigateToNoInternetConnectionPage()
        {
            var tokenStatus = await Provider.GetTokenStatus();
            if (tokenStatus != TokenStatus.BothNotExisting && !InternetConnectionHelper.IsOnNoInternetConnectionPage)
                await Service.NavigationService.NavigateAsync(ViewName.NoInternetConnectionPage);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try

            {
                base.OnNavigatedTo(parameters);

                if (parameters == null) return;

             // var mode=  parameters.GetNavigationMode ()
                var loginResultParams = NavigationParametersHelper.GetNavigationParameters<LoginResult>(parameters);
                var logoutNavParams = NavigationParametersHelper.GetNavigationParameters<LogoutNavParams>(parameters);

                AccountResponseDto accountData;

                if (logoutNavParams == null && loginResultParams == null)
                {
                    // Start up the app ("first time / previously logout pressed and app restarted")

                    // perform silent login if JwtToken valid
                    var silentLoginResult = await Provider.TrySilentLogin();

                    if (!silentLoginResult.Success)
                    {
                        // JwtToken and RefreshToken expired => navigate to GetStartedPage
                        await Service.NavigationService.NavigateAsync(ViewName.GetStarted);
                        return;
                    }
                    else
                    {
                        accountData = await Provider.GetAccountDataAsync();
                        if (accountData != null)
                        {
                            Provider.SetDisplayPreferences(accountData);

                            // save user email into SecureStorage
                            await _secureStorageService.SaveString(SecureStorageKey.DisplayName, accountData.FirstName + " " + accountData.LastName);

                            // TODO Remove commented if statement
                           if (!accountData.CompletedFirstUseWizard)
                            {
                                await Service.NavigationService.NavigateAsync(ViewName.WizardStartPage);
                                return;
                            }
                        }

                        if (string.IsNullOrEmpty(_currentViewName) || !_currentViewName.Equals(ViewName.IntegrationDetailPage))
                        {
                            _eventAggregator.GetEvent<InitializeTabViewEvent>().Publish(nameof(MainPageViewModel));
                            // _eventAggregator.GetEvent<InitializeTabViewEvent>().Publish(nameof(AnalysisPageViewModel));
                            //await Service.NavigationService.NavigateAsync(ViewName.WizardFinishSetupPage);    //ToDO
                            return;
                        }
                    }
                }

                if (loginResultParams != null)
                {
                    // User clicked the login button
                    if (!HasInternetConnection)
                    {
                        await NavigateToNoInternetConnectionPage();
                        return;
                    }

                    if (string.IsNullOrEmpty(_currentViewName) || !_currentViewName.Equals(ViewName.IntegrationDetailPage))
                    {
                        _eventAggregator.GetEvent<InitializeTabViewEvent>().Publish(nameof(MainPageViewModel));
                    }

                    return;
                }

                if (logoutNavParams != null)
                {
                    // User clicked the logout button
                    await Service.NavigationService.NavigateAsync(ViewName.GetStarted);
                }
                else
                {
                    accountData = await Provider.GetAccountDataAsync();
                    if (accountData != null)
                    {
                        if (!accountData.CompletedFirstUseWizard)
                        {
                            await Service.NavigationService.NavigateAsync(ViewName.WizardStartPage);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception in Content shell vm => OnNavigatedto" + ex.Message + " Stacktrace :  " + ex.StackTrace);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void Destroy()
        {
            base.Destroy();

            _eventAggregator.GetEvent<UnauthorizedEvent>().Unsubscribe(OnUnauthorizedEvent);
        }

        private void OnSelectTab(PageNavigationParams args)
        {
            if (Page != null)
            {
                Page.NavigateToTab(args);
            }
        }
    }
}