using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class DietPreferencesPageVmProvider : PageVmProviderBase, IDietPreferencesPageVmProvider
    {
        private readonly IUserDataService _userDataService;
        private readonly ILogger _logger;

        public DietPreferencesPageVmProvider(ILoggerFactory loggerFactory, IUserDataService userDataService, ITokenService tokenService) : base(tokenService)
        {
            _userDataService = userDataService;
            _logger = loggerFactory?.CreateLogger<DietPreferencesPageVmProvider>() ??
                      new NullLogger<DietPreferencesPageVmProvider>();
        }

        public async Task<ApiCallResult<DietPreferencesProxy>> GetDietPreferencesAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetDietPreferenceAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var dietPreferencesProxy = ProxyConverter.GetDietPreferencesProxy(apiCallResult.Value);

                return ApiCallResult<DietPreferencesProxy>.Ok(dietPreferencesProxy);
            }

            return ApiCallResult<DietPreferencesProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<DietPreferencesProxy>> CreateDietPreferencesAsnyc(DietPreferencesProxy dietPreferencesProxy, CancellationToken cancellationToken)
        {
            var dietPreferencesDto = ProxyConverter.GetDietPreferencesDto(dietPreferencesProxy);

            var apiCallResult = await _userDataService.CreateDietPreferenceAsync(dietPreferencesDto, cancellationToken);

            if (apiCallResult.Success)
            {
                dietPreferencesProxy = ProxyConverter.GetDietPreferencesProxy(apiCallResult.Value);

                return ApiCallResult<DietPreferencesProxy>.Ok(dietPreferencesProxy);
            }

            return ApiCallResult<DietPreferencesProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<DietPreferencesProxy>> UpdateDietPreferencesAsnyc(DietPreferencesProxy dietPreferencesProxy, CancellationToken cancellationToken)
        {
            var dietPreferencesDto = ProxyConverter.GetDietPreferencesDto(dietPreferencesProxy);

            var apiCallResult = await _userDataService.UpdateDietPreferenceAsync(dietPreferencesDto, cancellationToken);

            if (apiCallResult.Success)
            {
                dietPreferencesProxy = ProxyConverter.GetDietPreferencesProxy(apiCallResult.Value);

                return ApiCallResult<DietPreferencesProxy>.Ok(dietPreferencesProxy);
            }

            return ApiCallResult<DietPreferencesProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}