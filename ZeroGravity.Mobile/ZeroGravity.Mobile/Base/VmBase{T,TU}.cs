using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models;


namespace ZeroGravity.Mobile.Base
{
    public abstract class VmBase<TPage, TProvider, TViewModel> : VmBase<TPage>
        where TPage : IPage
        where TProvider : IPageVmProvider
    {
        protected TProvider Provider;
        private readonly IApiService _apiService;
        protected ILogger Logger;

        protected VmBase(IVmCommonService service, TProvider provider, ILoggerFactory loggerFactory, IApiService apiService, bool checkInternet = true) : base(service, checkInternet)
        {
            try
            {
                Provider = provider;
                _apiService = apiService;
                Logger = loggerFactory?.CreateLogger<TViewModel>() ?? new NullLogger<TViewModel>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e, e.Message);
            }
        }

        protected VmBase(IVmCommonService service, TProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, checkInternet)
        {
            Provider = provider;
            Logger = loggerFactory?.CreateLogger<TViewModel>() ?? new NullLogger<TViewModel>();

            try
            {
                if (App.DiContainerProvider != null)
                {
                    _apiService = (IApiService)App.DiContainerProvider.Resolve(typeof(IApiService));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e, e.Message);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            { 
            Logger.LogDebug("NavigatedTo");
            base.OnNavigatedTo(parameters);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);

            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            try
            {
                // Logger.LogDebug("NavigatedFrom");
                base.OnNavigatedFrom(parameters);

            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, ex.Message);

            }
        }


        protected async Task<bool> ValidateToken()
        {
            await Service.SemaphoreAsyncLockService.SemaphoreSlim.WaitAsync();

            try
            {
                var tokenStatus = await Provider.GetTokenStatus();

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{nameof(ValidateToken)} result: {tokenStatus}");
#endif

                switch (tokenStatus)
                {
                    case TokenStatus.BothNotExisting:
                        return false;
                    case TokenStatus.JwtValid:
                        return true;
                    case TokenStatus.RefreshTokenValid:
#if DEBUG
                        System.Diagnostics.Debug.WriteLine($"{nameof(ValidateToken)}: JWT expired, but RefreshToken is valid. Proceeding to get a new JWT...");
#endif
                        await RefreshTokenAsync();
                        return true;
                    case TokenStatus.Invalid:

                        if (Service.NavigationService != null)
                        {
                            // user is on a 'ContentPage'
                            await Service.DialogService.DisplayAlertAsync(AppResources.SessionTimedOut_Title, AppResources.SessionTimedOut_Msg, AppResources.Button_Ok);

                            await Service.NavigationService.NavigateAsync("/" + ViewName.Login);
                        }
                        else
                        {
                            // user is on a 'ContentView'
                            Service.EventAggregator.GetEvent<UnauthorizedEvent>().Publish();
                        }

                        return false;

                    case TokenStatus.Other:
                        try
                        {
                            await Service.NavigationService.GoBackToRootAsync();
                        }
                        catch (Exception e)
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message, AppResources.Button_Ok);
                            System.Diagnostics.Debug.WriteLine(e);
                            return false;
                        }

                        return false;

                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Logger.LogCritical(e, e.Message);
                return false;
            }

            finally
            {
                Service.SemaphoreAsyncLockService.SemaphoreSlim.Release();
            }
        }

        protected void CancelPendingRequest(CancellationTokenSource cts)
        {
            cts?.Cancel();
        }

        private async Task RefreshTokenAsync()
        {
            // get new JwtToken using refresh-token api
            var baseUrl = Common.ServerUrl;
            var api = "/accounts/refresh-token";
            var url = baseUrl + api;

            try
            {
                if (_apiService == null)
                {
                    Service.EventAggregator.GetEvent<RefreshTokenEvent>().Publish();
                    return;
                }
                var response = await _apiService.PostRx<LoginResponse>(url, CancellationToken.None);

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{nameof(RefreshTokenAsync)}: Obtained new JWT: {response.JwtToken}");
#endif

                await Provider.SaveNewJwt(response.JwtToken);
                await Provider.SaveNewRefreshToken(response.RefreshToken, response.RefreshTokenExpiration);
            }
            catch (HttpException ex)
            {
                if (ex.CustomErrorMessage == "Invalid token") // ToDo: use static string
                {
                    if (Service.NavigationService != null)
                    {
                        // user is on a 'ContentPage'
                        await Service.DialogService.DisplayAlertAsync(
                            AppResources.SessionTimedOut_Title,
                            AppResources.SessionTimedOut_Msg,
                            AppResources.Button_Ok);

                        await Service.NavigationService.NavigateAsync("/" + ViewName.Login);
                    }
                    else
                    {
                        // user is on a 'ContentView'
                        Service.EventAggregator.GetEvent<UnauthorizedEvent>().Publish();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }
        }



    }
}
