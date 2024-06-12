using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class AccountSecurityPageViewModel : VmBase<IAccountSecurityPage, IAccountSecurityPageVmProvider, AccountSecurityPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private CancellationTokenSource _cts;
        private readonly ITokenService _tokenService;

        public DelegateCommand ChangeEmailCommand { get; }
        public DelegateCommand ChangePasswordCommand { get; }
        public DelegateCommand DeleteAccountCommand { get; }
        public DelegateCommand LogoutCommand { get; }

        public AccountSecurityPageViewModel(IVmCommonService service, IAccountSecurityPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator, ISecureStorageService secureStorageService, ITokenService tokenService) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;
            _secureStorageService = secureStorageService;
            _tokenService = tokenService;
            ChangeEmailCommand = new DelegateCommand(ChangeEmailExecute);
            ChangePasswordCommand = new DelegateCommand(ChangePasswordExecute);
            DeleteAccountCommand = new DelegateCommand(DeleteAccountExecute);
            LogoutCommand = new DelegateCommand(LogoutExecute);
        }

        private void ChangeEmailExecute()
        {
            _eventAggregator.GetEvent<ChangeEmailPageEvent>().Publish();
        }

        private void ChangePasswordExecute()
        {
            _eventAggregator.GetEvent<ChangePasswordPageEvent>().Publish();
        }

        private void DeleteAccountExecute()
        {
            _eventAggregator.GetEvent<AccountDeletePageEvent>().Publish();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            IsBusy = true;

            if (await ValidateToken())
            {
                if (!HasInternetConnection)
                {
                    return;
                }

                var userEmail = await Provider.GetUserEmailFromSecureStorageAsync(_cts.Token);
                Email = userEmail;
            }

            IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private async void LogoutExecute()
        {
            try
            {
                IsBusy = true;
                await _tokenService.RemoveJwt();
                await _tokenService.RemoveRefreshToken();

                await _secureStorageService.Remove(SecureStorageKey.UserEmail);

                var navParams = NavigationParametersHelper.CreateNavigationParameter(new LogoutNavParams { IsLogoutRequested = true });

                // await Service.NavigationService.NavigateAsync("/" + ViewName.GetStarted, navParams);
                //await Service.NavigationService.NavigateAsync("/" + ViewName.ContentShellPage, navParams);

                var navigationParams = new PageNavigationParams
                {
                    DateTime = DateTime.Now,
                    PageName = ViewName.GetStarted
                };

                _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
            }
            catch (Exception e)
            {
                await Service.DialogService.DisplayAlertAsync("Error", e.Message, AppResources.Button_Ok);
                Logger.LogError(e, e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
    }
}