using Microsoft.Extensions.Logging;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class AboutPageViewModel : VmBase<IAboutPage, IAboutPageVmProvider, AboutPageViewModel>
    {
        public AboutPageViewModel(IVmCommonService service, IAboutPageVmProvider provider, ILoggerFactory loggerFactory)
            : base(service, provider, loggerFactory)
        {
        }
    }
}