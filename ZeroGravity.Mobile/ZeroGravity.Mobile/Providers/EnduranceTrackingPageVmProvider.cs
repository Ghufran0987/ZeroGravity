using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class EnduranceTrackingPageVmProvider : PageVmProviderBase, IEnduranceTrackingPageVmProvider
    {
        public EnduranceTrackingPageVmProvider(ITokenService tokenService) : base(tokenService)
        {
        }
    }
}
