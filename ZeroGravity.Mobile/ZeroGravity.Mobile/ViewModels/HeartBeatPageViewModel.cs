using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class HeartBeatPageViewModel : VmBase<IHeartBeatPage, IHeartBeatPageVmProvider, HeartBeatPageViewModel>
    {
        public HeartBeatPageViewModel(IVmCommonService service, IHeartBeatPageVmProvider provider, ILoggerFactory loggerFactory, IApiService apiService, bool checkInternet = true) : base(service, provider, loggerFactory, apiService, checkInternet)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //var showOverlay = Service.HoldingPagesSettingsService.ShouldShow(HoldingPageType.HeartBeat);
            //if (showOverlay)
            //{
            //    OpenOverlay();
            //}
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();

            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.HeartBeat);
        }
    }
}
