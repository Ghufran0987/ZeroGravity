using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Providers
{
    public class RegisterPageVmProvider : PageVmProviderBase, IRegisterPageVmProvider
    {
        private readonly IRegisterService _registerService;
        private readonly ILogger _logger;

        public RegisterPageVmProvider(ILoggerFactory loggerFactory, IRegisterService registerService, ITokenService tokenService) : base(tokenService)
        {
            _registerService = registerService;
            _logger = loggerFactory?.CreateLogger<RegisterPageVmProvider>() ?? new NullLogger<RegisterPageVmProvider>();
        }

        public async Task<RegisterResult> RegisterAsync(string email, string password, string confirmPassword, bool acceptTerms, bool wantsNewsletter, CancellationToken cancellationToken)
        {
            return await _registerService.RegisterAsync(email, password, confirmPassword, acceptTerms, wantsNewsletter, cancellationToken);
        }
    }

    public class LoginAlertPageVmProvider : PageVmProviderBase, ILoginAlertPageVmProvider
    {
        private readonly IRegisterService _registerService;
        private readonly ILogger _logger;

        public LoginAlertPageVmProvider(ILoggerFactory loggerFactory, IRegisterService registerService, ITokenService tokenService) : base(tokenService)
        {
            _registerService = registerService;
            _logger = loggerFactory?.CreateLogger<LoginAlertPageVmProvider>() ?? new NullLogger<LoginAlertPageVmProvider>();
        }

      
    }
}
