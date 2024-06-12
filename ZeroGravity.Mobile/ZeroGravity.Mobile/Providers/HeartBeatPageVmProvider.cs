using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class HeartBeatPageVmProvider : PageVmProviderBase, IHeartBeatPageVmProvider
    {
        public HeartBeatPageVmProvider(ITokenService tokenService) : base(tokenService)
        {
        }
    }
}
