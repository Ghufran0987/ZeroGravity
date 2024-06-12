using System.Threading;
using System.Threading.Tasks;
using Prism.Events;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class IntegrationDetailPageVmProvider : PageVmProviderBase, IIntegrationDetailPageVmProvider
    {
        private readonly INativeBrowserService _nativeBrowserService;
        private readonly IFitbitIntegrationService _fitbitIntegrationService;
        private readonly IEventAggregator _eventAggregator;

        public IntegrationDetailPageVmProvider(ITokenService tokenService, INativeBrowserService nativeBrowserService, IFitbitIntegrationService fitbitIntegrationService) : base(tokenService)
        {
            _nativeBrowserService = nativeBrowserService;
            _fitbitIntegrationService = fitbitIntegrationService;
        }

        public void OpenNativeWebBrowser(string url)
        {
            _nativeBrowserService.LaunchNativeEmbeddedBrowser(url);
        }

        public async Task<ApiCallResult<FitbitAccountProxy>> GetFitbitAccountAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _fitbitIntegrationService.GetFitbitAccountAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var fitbitAccountProxy = ProxyConverter.GetFitbitAccountProxy(apiCallResult.Value);

                return ApiCallResult<FitbitAccountProxy>.Ok(fitbitAccountProxy);
            }

            return ApiCallResult<FitbitAccountProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}