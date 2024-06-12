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
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class TermsAndPrivacyPageViewModel : VmBase<ITermsAndPrivacyPage, ITermsAndPrivacyPageVmProvider, TermsAndPrivacyPageViewModel>
    {
        public TermsAndPrivacyPageViewModel(IVmCommonService service, ITermsAndPrivacyPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            Title = AppResources.TermsAndPrivacyPageTitle;
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
            RegisterCommand = new DelegateCommand(RegisterExecute);
        }


        public DelegateCommand RegisterCommand { get; }

        private ImageSource _logoImageSource;
        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (Page != null)
            {
                var navParams = NavigationParametersHelper.GetNavigationParameters<TermsAndPrivacyNavParams>(parameters);

                if (navParams.TabTitle.Equals(AppResources.TermsAndPrivacy_PrivacyTabTitle))
                {
                    Page.SetTab(AppResources.TermsAndPrivacy_PrivacyTabTitle);
                }
                else
                {
                    Page.SetTab(AppResources.TermsAndPrivacy_TermsTabTitle);
                }
            }

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


        private async void RegisterExecute()
        {
        }

    }
}
