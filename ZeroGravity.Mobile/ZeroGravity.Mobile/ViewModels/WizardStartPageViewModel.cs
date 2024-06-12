using System;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardStartPageViewModel : VmBase<IWizardStartPage, IWizardStartPageVmProvider, WizardStartPageViewModel>
    {
        public WizardStartPageViewModel(IVmCommonService service, IWizardStartPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            GoToLoginPageCommand = new DelegateCommand(GoToLoginPageExecute);
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
            ArrowBackImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Arrow-Backwards.png");
        }

        private async void GoToLoginPageExecute()
        {
            try
            {
                //true indicated its navigated from normal flow not from back navigation
                var navParams = NavigationParametersHelper.CreateNavigationParameter(new BooleanNavParams
                { IsTrue = true });
                await Service.NavigationService.NavigateAsync(ViewName.WizardNewPage, navParams);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        private ImageSource _logoImageSource;

        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        private ImageSource _arrowBackImageSource;

        public ImageSource ArrowBackImageSource
        {
            get => _arrowBackImageSource;
            set => SetProperty(ref _arrowBackImageSource, value);
        }

        public DelegateCommand GoToLoginPageCommand { get; }
    }
}