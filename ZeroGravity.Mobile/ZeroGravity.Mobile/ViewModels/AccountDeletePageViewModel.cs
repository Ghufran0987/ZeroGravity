using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class AccountDeletePageViewModel : VmBase<IAccountDeletePage, IAccountDeletePageVmProvider, AccountDeletePageViewModel>
    {
        private readonly ITokenService _tokenService;
        public DelegateCommand DeleteAccountForeverCommand { get; }

        private CancellationTokenSource _cts;

        public AccountDeletePageViewModel(IVmCommonService service, IAccountDeletePageVmProvider provider, ILoggerFactory loggerFactory,
            ITokenService tokenService) : base(service, provider, loggerFactory)
        {
            _tokenService = tokenService;
            DeleteAccountForeverCommand = new DelegateCommand(DeleteAccountExecute);
        }

        private async void DeleteAccountExecute()
        {
            IsBusy = true;

            if (await ValidateToken())
            {
                _cts = new CancellationTokenSource();

                var result = await Provider.ConfirmPassword(Password, _cts.Token);

                if (result.Success)
                {
                    // proceed to delete acc
                    var apiCallResult = await Provider.DeleteAccount(_cts.Token);
                    await _tokenService.RemoveJwt();
                    await _tokenService.RemoveRefreshToken();
                    await Service.DialogService.DisplayAlertAsync(AppResources.Account_Deleted_Title,
                        apiCallResult.Value, AppResources.Button_Ok);

                    await Service.NavigationService.NavigateAsync("/" + ViewName.Register);

                }
                else
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.Password_Invalid_Title, AppResources.Password_Invalid, AppResources.Button_Ok);
                }
            }

            IsBusy = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Title = AppResources.Account_Delete;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

    }
}
