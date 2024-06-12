using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class FirstUseWizardPageVmProvider : PageVmProviderBase, IFirstUseWizardPageVmProvider
    {
        private readonly ILogger _logger;
        private readonly IUserDataService _userDataService;


        public FirstUseWizardPageVmProvider(ILoggerFactory loggerFactory, IUserDataService userDataService, ITokenService tokenService) : base(tokenService)
        {
            _userDataService = userDataService;

            _logger = loggerFactory?.CreateLogger<FirstUseWizardPageVmProvider>() ??
                      new NullLogger<FirstUseWizardPageVmProvider>();
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsnyc(CancellationToken token)
        {
            var apiCallResult = await _userDataService.GetAccountDataAsync(token);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage);
        }

        public async Task<ApiCallResult<bool>> UpdateAccountDataAsnyc(UpdateWizardRequestDto updateWizardRequestDto, CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.UpdateWizardStateAsync(updateWizardRequestDto, cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<bool>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<bool>.Error(apiCallResult.ErrorMessage);
        }
    }
}

