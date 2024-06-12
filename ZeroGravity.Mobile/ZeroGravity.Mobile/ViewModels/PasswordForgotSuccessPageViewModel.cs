using System;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class PasswordForgotSuccessPageViewModel : VmBase<IPasswordForgotSuccessPage, IPasswordForgotSuccessPageVmProvider, PasswordForgotSuccessPageViewModel>
    {
        public PasswordForgotSuccessPageViewModel(IVmCommonService service, IPasswordForgotSuccessPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            GoToLoginPageCommand = new DelegateCommand(GoToLoginPageExecute);
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
            ArrowBackImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Arrow-Backwards.png");
        }

        private async void GoToLoginPageExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.GetStarted);
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