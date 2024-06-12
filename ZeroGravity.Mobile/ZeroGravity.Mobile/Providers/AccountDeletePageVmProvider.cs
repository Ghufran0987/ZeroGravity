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
    public class AccountDeletePageVmProvider : PageVmProviderBase, IAccountDeletePageVmProvider
    {
        private readonly IAccountSecurityService _accountSecurityService;
        private readonly ILogger _logger;

        public AccountDeletePageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IAccountSecurityService accountSecurityService) : base(tokenService)
        {
            _accountSecurityService = accountSecurityService;
            _logger = loggerFactory?.CreateLogger<AccountDeletePageVmProvider>() ?? new NullLogger<AccountDeletePageVmProvider>();
        }

        public async Task<ApiCallResult<bool>> ConfirmPassword(string password, CancellationToken cancellationToken)
        {
            var result = await _accountSecurityService.ConfirmPassword(password, cancellationToken);

            if (!result.Success)
            {
                return ApiCallResult<bool>.Error(result.Message, result.ErrorReason);
            }

            return ApiCallResult<bool>.Ok(true);
        }
        
        public async Task<ApiCallResult<string>> DeleteAccount(CancellationToken cancellationToken)
        {
            var apiCallResult = await _accountSecurityService.DeleteAccount(cancellationToken);
            return apiCallResult;
        }


    }
}
