using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class DemoPageVmProvider : PageVmProviderBase, IDemoPageVmProvider
    {
        public DemoPageVmProvider(ITokenService tokenService) : base(tokenService)
        {
        }
    }
}
