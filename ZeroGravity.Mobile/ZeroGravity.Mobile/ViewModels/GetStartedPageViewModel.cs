using System;
using System.Threading;
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

namespace ZeroGravity.Mobile.ViewModels
{
    public class GetStartedPageViewModel : VmBase<IGetStartedPage, IGetStartedPageVmProvider, GetStartedPageViewModel>
    {
        private LoginProxy _proxy;
        private CancellationTokenSource _cts;
        public DelegateCommand LoginCommand { get; }
        public DelegateCommand ForgotPasswordCommand { get; }
        public DelegateCommand JoinMibokoCommand { get; }
        public DelegateCommand AppleCommand { get; }
        public DelegateCommand GoogleCommand { get; }
        public DelegateCommand SignInWithEmailCommand { get; }
        public DelegateCommand JoinWithEmailCommand { get; }

        public DelegateCommand PrivacyCommand { get; }

        public LoginProxy Proxy
        {
            get => _proxy;
            set => SetProperty(ref _proxy, value);
        }

        private bool _isLoginSuccess = false;

        public bool IsLoginSuccess
        {
            get => _isLoginSuccess;
            set => SetProperty(ref _isLoginSuccess, value);
        }


        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private ImageSource _logoImageSource;

        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        public GetStartedPageViewModel(IVmCommonService service, IGetStartedPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            Logger.LogInformation("Hello from GetStartedPageViewModel!");

            AppleCommand = new DelegateCommand(AppleExecute);
            GoogleCommand = new DelegateCommand(GoogleExecute);
            SignInWithEmailCommand = new DelegateCommand(SignInWithEmailExecute);

            JoinWithEmailCommand = new DelegateCommand(JoinWithEmailExecute);

            PrivacyCommand = new DelegateCommand(PrivacyExecute);
            LoginCommand = new DelegateCommand(LoginExecute);
            ForgotPasswordCommand = new DelegateCommand(ForgotPasswordExecute);
            JoinMibokoCommand = new DelegateCommand(JoinMiboko);

            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        private async void SignInWithEmailExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.Login);
        }

        private async void PrivacyExecute()
        {
            var termsAndPrivacyNavParams = new TermsAndPrivacyNavParams { TabTitle = AppResources.TermsAndPrivacy_TermsTabTitle };
            var navParams = NavigationParametersHelper.CreateNavigationParameter(termsAndPrivacyNavParams);

            await Service.NavigationService.NavigateAsync(ViewName.TermsAndPrivacyPage, navParams);
        }

        private async void JoinWithEmailExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.Register);
        }

        private async void AppleExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.LoginAlert);
            //await Service.DialogService.DisplayAlertAsync("Sign in with Apple", "Coming soon!", AppResources.Button_Ok);
        }

        private async void GoogleExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.LoginAlert);
            //await Service.DialogService.DisplayAlertAsync("Sign in with Google", "Coming soon!", AppResources.Button_Ok);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsBusy = true;
            Proxy = await Provider.GetLoginProxy();
            IsBusy = false;
            IsLoginSuccess = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            // in case user decides to NavigateFrom LoginPage while login request task has not finished yet
            CancelPendingRequest(_cts);
        }

        private async void LoginExecute()
        {
            IsBusy = true;

            _cts = new CancellationTokenSource();

#if DEBUG
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
            {
                Email = Proxy.Email;
                Password = Proxy.Password;
            }
#endif
            

            if (!HasInternetConnection)
            {
                Logger.LogInformation($"User {Email} had no Internet connection when attempting to login.");
                return;
            }

            var loginResult = await Provider.LoginAsync(Email, Password, _cts.Token, Proxy.SaveLogin);

            if (loginResult.Success)
            {
                Logger.LogInformation($"User: {Proxy.Email} successfully logged in.");
                Email = string.Empty;
                Password = string.Empty;
            }
            else
            {
                IsBusy = false;

                Logger.LogInformation($"Login failed for user {Email}.\nMessage: {loginResult.Message}\nReason: {loginResult.ErrorReason}");

                if (loginResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || loginResult.ErrorReason == ErrorReason.TimeOut)
                {
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.Login_Title, loginResult.Message, AppResources.Button_Ok);
               
                return;
            }

            if (Proxy.SaveLogin)
            {
                // ToDo: Save user's email into SecureStorage so it will be provided for the user next time on the login screen
            }

            // navigate to MainPage
            var navParams = NavigationParametersHelper.CreateNavigationParameter(loginResult);
            try
            {
                // get personal data from server
                var accountDataApiCallResult = await Provider.GetAccountDataAsnyc(_cts.Token);

                if (accountDataApiCallResult.Success)
                {
                    var accountData = accountDataApiCallResult.Value;
                    if (accountData.CompletedFirstUseWizard)
                    {
                        // perform absolute navigation:
                        // ==> causes the app to be closed when the device's "back button" is pressed instead of navigating to the previous view
                        await Service.NavigationService.NavigateAsync("/" + ViewName.ContentShellPage, navParams);
                    }
                    else
                    {
                        await Service.NavigationService.NavigateAsync(ViewName.WizardStartPage);
                    }
                }
                else
                {
                    Logger.LogInformation("Failed to load PersonalData");
                    await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                        accountDataApiCallResult.ErrorMessage, AppResources.Button_Ok);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.LogError(e.Message, e);
            }

            IsBusy = false;
        }

        private async void JoinMiboko()
        {
            await Service.NavigationService.NavigateAsync(ViewName.Register);
        }

        private async void ForgotPasswordExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.PasswordForgot);
        }
    }
}