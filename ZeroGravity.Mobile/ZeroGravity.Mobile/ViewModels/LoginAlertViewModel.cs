using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class LoginAlertViewModel : VmBase<ILoginAlert, ILoginAlertPageVmProvider, LoginAlertViewModel>
    {
        private CancellationTokenSource _cts;

        public LoginAlertViewModel(IVmCommonService service, ILoginAlertPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            Logger.LogInformation("Hello from LoginAlertViewModel!");
          
            SignInCommand = new DelegateCommand(SignInExecute);
            TermsCommand = new DelegateCommand(TermsExecute);
            PrivacyCommand = new DelegateCommand(PrivacyExecute);

            JoinWithAppleCommand = new DelegateCommand(JoinWithAppleExecute);
            JoinWithGoogleCommand = new DelegateCommand(JoinWithGoogleExecute);
            JoinWithEmailCommand = new DelegateCommand(JoinWithEmailExecute);
            BottomBackArrowCommand = new DelegateCommand(BottomBackArrowExecute);

            //Title = AppResources.Register_Title;

            BackgroundImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background_uni.png");
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }


        public DelegateCommand SignInCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand TermsCommand { get; }
        public DelegateCommand PrivacyCommand { get; }

        public DelegateCommand JoinWithAppleCommand { get; }
        public DelegateCommand JoinWithGoogleCommand { get; }
        public DelegateCommand JoinWithEmailCommand { get; }

        public DelegateCommand BottomBackArrowCommand { get; }


        private async void JoinWithAppleExecute()
        {
            await Service.DialogService.DisplayAlertAsync("Join in with Apple", "Coming soon.", AppResources.Button_Ok);
        }

        private async void JoinWithGoogleExecute()
        {
            await Service.DialogService.DisplayAlertAsync("Join in with Google", "Coming soon.", AppResources.Button_Ok);
        }

        private void JoinWithEmailExecute()
        {
            AreJoinButtonsVisible = false;
            AreEmailAndPasswordInputFieldsVisible = true;
        }

        private void BottomBackArrowExecute()
        {
            InitControlsVisibility();
            Email = string.Empty;
            Password = string.Empty;
        }


      


        private async void PrivacyExecute()
        {
            var termsAndPrivacyNavParams = new TermsAndPrivacyNavParams {TabTitle = AppResources.TermsAndPrivacy_PrivacyTabTitle};
            var navParams = NavigationParametersHelper.CreateNavigationParameter(termsAndPrivacyNavParams);

            await Service.NavigationService.NavigateAsync(ViewName.TermsAndPrivacyPage, navParams);
        }

        private async void TermsExecute()
        {
            var termsAndPrivacyNavParams = new TermsAndPrivacyNavParams { TabTitle = AppResources.TermsAndPrivacy_TermsTabTitle};
            var navParams = NavigationParametersHelper.CreateNavigationParameter(termsAndPrivacyNavParams);
            await Service.NavigationService.NavigateAsync(ViewName.TermsAndPrivacyPage, navParams);
        }


        private async void SignInExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.Login);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            InitControlsVisibility();
        }

        private void InitControlsVisibility()
        {
            AreJoinButtonsVisible = true;
            AreEmailAndPasswordInputFieldsVisible = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            // in case user decides to NavigateFrom LoginAlert while login request task has not finished yet
            CancelPendingRequest(_cts);
        }


        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _acceptTermsAndPrivacy;
        public bool AcceptTermsAndPrivacy
        {
            get { return _acceptTermsAndPrivacy; }
            set { SetProperty(ref _acceptTermsAndPrivacy, value); }
        }

        private bool _newsletterSend;

        public bool NewsletterSend
        {
            get { return _newsletterSend; }
            set { SetProperty(ref _newsletterSend, value); }
        }


        private string _termsAndPrivacy;

        public string TermsAndPrivacy
        {
            get { return _termsAndPrivacy; }
            set { SetProperty(ref _termsAndPrivacy, value); }
        }

        private ImageSource _backgroundImageSource;
        public ImageSource BackgroundImageSource
        {
            get => _backgroundImageSource;
            set => SetProperty(ref _backgroundImageSource, value);
        }

        private ImageSource _logoImageSource;
        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        private bool _areJoinButtonsVisible;
        public bool AreJoinButtonsVisible
        {
            get { return _areJoinButtonsVisible; }
            set { SetProperty(ref _areJoinButtonsVisible, value); }
        }

        private bool _areEmailAndPasswordInputFieldsVisible;
        public bool AreEmailAndPasswordInputFieldsVisible
        {
            get { return _areEmailAndPasswordInputFieldsVisible; }
            set { SetProperty(ref _areEmailAndPasswordInputFieldsVisible, value); }
        }

    }
}