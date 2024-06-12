using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class PasswordForgotSuccessPageVmProvider: PageVmProviderBase, IPasswordForgotSuccessPageVmProvider
    {
        private readonly ILogger _logger;

        public PasswordForgotSuccessPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<PasswordForgotSuccessPageVmProvider>() ?? new NullLogger<PasswordForgotSuccessPageVmProvider>();
        }
    }
}
