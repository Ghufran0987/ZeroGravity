using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class AccountOverviewPageVmProvider : PageVmProviderBase, IAccountOverviewPageVmProvider
    {
        private readonly ILogger _logger;

        public AccountOverviewPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<AccountOverviewPageVmProvider>() ??
                      new NullLogger<AccountOverviewPageVmProvider>();
        }
    }
}