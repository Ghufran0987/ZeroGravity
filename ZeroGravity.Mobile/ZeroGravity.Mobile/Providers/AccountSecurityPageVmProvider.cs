using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class AccountSecurityPageVmProvider : PageVmProviderBase, IAccountSecurityPageVmProvider
    {
        private readonly IUserDataService _userDataService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILogger _logger;

        public AccountSecurityPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService,
            IUserDataService userDataService, ISecureStorageService secureStorageService) : base(tokenService)
        {
            _userDataService = userDataService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<AccountSecurityPageVmProvider>() ??
                      new NullLogger<AccountSecurityPageVmProvider>();
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetAccountDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<string> GetUserEmailFromSecureStorageAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userEmail = await _secureStorageService.LoadString(SecureStorageKey.UserEmail);

            if (string.IsNullOrEmpty(userEmail))
            {
                // try get from server
                var apiCall = await GetAccountDataAsync(cancellationToken);
                if (apiCall.Success)
                {
                    return apiCall.Value.Email;
                }

                return string.Empty;
            }

            return userEmail;
        }
    }
}