using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class ChangePasswordPageVmProvider : PageVmProviderBase, IChangePasswordPageVmProvider
    {
        private readonly IAccountSecurityService _accountSecurityService;

        public ChangePasswordPageVmProvider(ITokenService tokenService, IAccountSecurityService accountSecurityService)
            : base(tokenService)
        {
            _accountSecurityService = accountSecurityService;
        }

        public async Task<ApiCallResult<bool>> ConfirmPassword(string password, CancellationToken cancellationToken)
        {
            var result = await _accountSecurityService.ConfirmPassword(password, cancellationToken);

            if (!result.Success) return ApiCallResult<bool>.Error(result.Message);

            return ApiCallResult<bool>.Ok(true);
        }

        public async Task<ApiCallResult<string>> ChangePassword(string oldPassword, string newPassword,
            CancellationToken cancellationToken)
        {
            var result = await _accountSecurityService.ChangePassword(oldPassword, newPassword, cancellationToken);

            if (result.Success) return ApiCallResult<string>.Ok(result.Value);

            return ApiCallResult<string>.Error(result.ErrorMessage);
        }
    }
}