using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ChangeEmailPageViewModel : VmBase<IChangeEmailPage, IChangeEmailPageVmProvider, ChangeEmailPageViewModel>
    {
        private CancellationTokenSource _cts;

        public Command SaveCommand { get; }

        public ChangeEmailPageViewModel(IVmCommonService service, IChangeEmailPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveCommand = new Command(SaveExecute);
        }

        private async void SaveExecute()
        {
            IsBusy = true;

            if (await ValidateToken())
            {
                if (string.IsNullOrEmpty(_newEmail) || string.IsNullOrEmpty(_password))
                {
                    // not ok
                    return;
                }

                _cts = new CancellationTokenSource();

                var result = await Provider.ConfirmPassword(Password, _cts.Token);

                if (result.Success)
                {
                    // tell user to check new mail
                    var apiCallResult = await Provider.ChangeEmail(NewEmail, _cts.Token);

                    await Service.DialogService.DisplayAlertAsync(AppResources.Email_Change_Title, apiCallResult.Value, AppResources.Button_Ok);
                    await Service.NavigationService.GoBackAsync();
                }
                else
                {
                    // wrong password
                    await Service.DialogService.DisplayAlertAsync(AppResources.Password_Invalid_Title, AppResources.Password_Invalid, AppResources.Button_Ok);
                }
            }

            IsBusy = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Title = AppResources.Email_Change_Title;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private string _newEmail;

        public string NewEmail
        {
            get { return _newEmail; }
            set { SetProperty(ref _newEmail, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

    }
}
