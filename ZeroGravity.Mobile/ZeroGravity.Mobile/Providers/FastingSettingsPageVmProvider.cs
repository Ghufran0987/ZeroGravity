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
    public class FastingSettingsPageVmProvider : PageVmProviderBase, IFastingSettingsPageVmProvider
    {
        private readonly IFastingSettingsService _fastingSettingsService;
        private readonly ILogger _logger;

        public FastingSettingsPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IFastingSettingsService fastingSettingsService) : base(tokenService)
        {
            _fastingSettingsService = fastingSettingsService;
            _logger = loggerFactory?.CreateLogger<FastingSettingsPageVmProvider>() ??
                      new NullLogger<FastingSettingsPageVmProvider>();
        }

        public async Task<ApiCallResult<FastingSettingProxy>> GetFastingSettingAsync(CancellationToken cancellationToken)
        {
            var apiCallResult = await _fastingSettingsService.GetFastingSettingByIdAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var fastingSettingProxy = ProxyConverter.GetFastingSettingProxy(apiCallResult.Value);

                return ApiCallResult<FastingSettingProxy>.Ok(fastingSettingProxy);
            }

            return ApiCallResult<FastingSettingProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<FastingSettingProxy>> CreateFastingDataAsnyc(FastingSettingProxy fastingSettingProxy, CancellationToken cancellationToken)
        {
            var fastingSettingDto = ProxyConverter.GetFastingSettingDto(fastingSettingProxy);

            var apiCallResult = await _fastingSettingsService.CreateFastingSettingAsync(fastingSettingDto, cancellationToken);

            if (apiCallResult.Success)
            {
                fastingSettingProxy = ProxyConverter.GetFastingSettingProxy(apiCallResult.Value);

                return ApiCallResult<FastingSettingProxy>.Ok(fastingSettingProxy);
            }

            return ApiCallResult<FastingSettingProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<FastingSettingProxy>> UpdateFastingSettingAsync(FastingSettingProxy fastingSettingProxy, CancellationToken cancellationToken)
        {
            var fastingSettingDto = ProxyConverter.GetFastingSettingDto(fastingSettingProxy);

            var apiCallResult = await _fastingSettingsService.UpdateFastingSettingAsync(fastingSettingDto, cancellationToken);

            if (apiCallResult.Success)
            {
                fastingSettingProxy = ProxyConverter.GetFastingSettingProxy(apiCallResult.Value);

                return ApiCallResult<FastingSettingProxy>.Ok(fastingSettingProxy);
            }

            return ApiCallResult<FastingSettingProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}