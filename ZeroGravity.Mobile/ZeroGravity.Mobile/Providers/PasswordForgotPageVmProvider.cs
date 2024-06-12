using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class PasswordForgotPageVmProvider : PageVmProviderBase, IPasswordForgotPageVmProvider
    {
        private readonly IPasswordForgotService _passwordForgotService;
        private readonly ILogger _logger;

        public PasswordForgotPageVmProvider(ILoggerFactory loggerFactory, IPasswordForgotService passwordForgotService, ITokenService tokenService) : base(tokenService)
        {
            _passwordForgotService = passwordForgotService;
            _logger = loggerFactory?.CreateLogger<PasswordForgotPageVmProvider>() ?? new NullLogger<PasswordForgotPageVmProvider>();
        }
        public async Task<PasswordForgotResult> RequestForgetLinkAsync(string email, CancellationToken cancellationToken)
        {
            return await _passwordForgotService.RequestForgetLinkAsync(email, cancellationToken);
        }
    }
}
