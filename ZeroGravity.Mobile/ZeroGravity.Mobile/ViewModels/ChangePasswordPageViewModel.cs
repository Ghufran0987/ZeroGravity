using Prism.Commands;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ChangePasswordPageViewModel : VmBase<IChangePasswordPage, IChangePasswordPageVmProvider, ChangePasswordPageViewModel>
    {
        private CancellationTokenSource _cts;

        public DelegateCommand SaveCommand { get; }

        public ChangePasswordPageViewModel(IVmCommonService service, IChangePasswordPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveCommand = new DelegateCommand(SaveExecute);
        }

        private async void SaveExecute()
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Password_Current_Required_Title, AppResources.Password_Current_Required_Message, AppResources.Button_Ok);
                return;
            }

            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPasswordRepeat))
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Password_New_Required_Title, AppResources.Password_New_Required_Message, AppResources.Button_Ok);
                return;
            }

            if (NewPassword != NewPasswordRepeat)
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Password_New_Match_Error_Title, AppResources.Password_New_Match_Error_Message, AppResources.Button_Ok);
                ClearEntries();
                return;
            }

            if (NewPassword.Length < 6)
            {
                var msg = string.Format(AppResources.Password_Length_Message, AccountAndPasswordConstants.PasswordMinimumLength);
                await Service.DialogService.DisplayAlertAsync(AppResources.Password_Length_Title, msg, AppResources.Button_Ok);
                ClearEntries();
                return;
            }

            IsBusy = true;

            if (await ValidateToken())
            {
                _cts = new CancellationTokenSource();

                var apiCallResult = await Provider.ConfirmPassword(OldPassword, _cts.Token);

                if (apiCallResult.Success)
                {
                    var result = await Provider.ChangePassword(OldPassword, NewPassword, _cts.Token);

                    await Service.DialogService.DisplayAlertAsync(AppResources.Password_Change, result.Value, AppResources.Button_Ok);
                    await Service.NavigationService.GoBackAsync();
                }
                else
                {
                    // wrong (old) password
                    await Service.DialogService.DisplayAlertAsync(AppResources.Password_Invalid_Title, AppResources.Password_Invalid, AppResources.Button_Ok);
                    OldPassword = null;
                }
            }

            IsBusy = false;
        }

        private void ClearEntries()
        {
            NewPassword = null;
            NewPasswordRepeat = null;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Title = AppResources.Password_Change;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }

        private string _newPasswordRepeat;

        public string NewPasswordRepeat
        {
            get { return _newPasswordRepeat; }
            set { SetProperty(ref _newPasswordRepeat, value); }
        }


    }
}
