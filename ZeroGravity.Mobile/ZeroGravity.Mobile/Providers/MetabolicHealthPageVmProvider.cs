using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class MetabolicHealthPageVmProvider : PageVmProviderBase, IMetabolicHealthPageVmProvider
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILogger _logger;
        private readonly IGlucoseService _glucoseService;

        public MetabolicHealthPageVmProvider(ISecureStorageService secureStorageService, ILoggerFactory loggerFactory, ITokenService tokenService, IGlucoseService glucoseService) : base(tokenService)
        {
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<MetabolicHealthPageVmProvider>() ?? new NullLogger<MetabolicHealthPageVmProvider>();
            _glucoseService = glucoseService;
        }

        public async Task<ApiCallResult<GlucoseBaseProxy>> SaveGlucoseAsync(GlucoseBaseProxy proxy, CancellationToken cancellationToken)
        {
            if (proxy is GlucoseManualProxy)
            {
                var glucoseManualProxy = (GlucoseManualProxy) proxy;

                proxy.DateTime = DateTimeHelper.AddTimeSpanToDateTime(proxy.DateTime, glucoseManualProxy.MeasurementTime);
            }

            var glucoseDataDto = ProxyConverter.GetGlucoseDataDto(proxy);
            // DateTime sollte nun in UTC im Dto stehen

            var apiCallResult = await _glucoseService.SaveGlucoseAsync(glucoseDataDto, cancellationToken);

            if (!apiCallResult.Success)
            {
                return ApiCallResult<GlucoseBaseProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
            }

            var glucoseProxy = ProxyConverter.GetGlucoseDataProxy(apiCallResult.Value);

            return ApiCallResult<GlucoseBaseProxy>.Ok(glucoseProxy);
        }
    }
}
