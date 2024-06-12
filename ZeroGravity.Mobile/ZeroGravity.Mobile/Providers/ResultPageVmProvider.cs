using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class ResultPageVmProvider : PageVmProviderBase, IResultPageVmProvider
    {
        public ResultPageVmProvider(ITokenService tokenService) : base(tokenService)
        {
        }
    }
}
