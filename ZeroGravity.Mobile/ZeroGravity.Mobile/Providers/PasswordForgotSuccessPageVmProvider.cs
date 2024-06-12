using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class RegisterSuccessPageVmProvider : PageVmProviderBase, IRegisterSuccessPageVmProvider
    {
        private readonly ILogger _logger;

        public RegisterSuccessPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<RegisterSuccessPageVmProvider>() ?? new NullLogger<RegisterSuccessPageVmProvider>();
        }
    }

    public class WizardStartPageVmProvider : PageVmProviderBase, IWizardStartPageVmProvider
    {
        private readonly ILogger _logger;

        public WizardStartPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<WizardStartPageVmProvider>() ?? new NullLogger<WizardStartPageVmProvider>();
        }
    }
}
