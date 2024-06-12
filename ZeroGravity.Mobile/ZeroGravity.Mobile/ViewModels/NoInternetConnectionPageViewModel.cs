using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class NoInternetConnectionPageViewModel : VmBase<INoInternetConnectionPage, INoInternetConnectionPageVmProvider, NoInternetConnectionPageViewModel>
    {
        public Command GoBackCommand { get; }


        public NoInternetConnectionPageViewModel(IVmCommonService service, INoInternetConnectionPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory, false)
        {
            GoBackCommand = new Command(GoBackExecute);
        }

        private async void GoBackExecute()
        {
            if (HasInternetConnection)
            {
                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                IsBusy = true;
                await Task.Delay(500);
                IsBusy = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InternetConnectionHelper.IsOnNoInternetConnectionPage = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            InternetConnectionHelper.IsOnNoInternetConnectionPage = false;
        }
    }
}
