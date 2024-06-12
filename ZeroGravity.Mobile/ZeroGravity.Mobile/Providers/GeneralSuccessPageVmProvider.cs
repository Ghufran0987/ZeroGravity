using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class GeneralSuccessPageVmProvider : PageVmProviderBase, IGeneralSuccessPageVmProvider
    {
        private readonly ILogger _logger;
        public GeneralSuccessPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<GeneralSuccessPageVmProvider>() ?? new NullLogger<GeneralSuccessPageVmProvider>();
        }
    }
}
