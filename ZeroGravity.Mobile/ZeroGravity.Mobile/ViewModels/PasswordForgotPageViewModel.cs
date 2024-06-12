using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class PasswordForgotPageViewModel : VmBase<IPasswordForgotPage, IPasswordForgotPageVmProvider, PasswordForgotPageViewModel>
    {
       
        private CancellationTokenSource _cts;

        public DelegateCommand PasswordForgotCommand { get; }

        public PasswordForgotPageViewModel(IVmCommonService service, IPasswordForgotPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            //Logger.LogInformation("Hello from PasswordForgotPageViewModel!");

            PasswordForgotCommand = new DelegateCommand(PasswordForgotExecute);

            //Title = AppResources.PasswordForgot;
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        private async void PasswordForgotExecute()
        {
            _cts = new CancellationTokenSource();

            if (string.IsNullOrEmpty(_email))
            {
                // not ok
                return;
            }

            IsBusy = true;

            var result = await Provider.RequestForgetLinkAsync(Email, _cts.Token);

            if (result.Success)
            {
                //await Service.DialogService.DisplayAlertAsync(AppResources.PasswordForgot, result.Message, AppResources.Button_Ok);
                await Service.NavigationService.NavigateAsync(ViewName.PasswordForgotSuccessPage);
                Email = string.Empty;
            }
            else
            {
                if (result.ErrorReason != ErrorReason.TaskCancelledByUserOperation)
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.PasswordReset_Error, result.Message, AppResources.Button_Ok);
                }
            }
            
            IsBusy = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }
        private ImageSource _logoImageSource;
        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
    }
}
