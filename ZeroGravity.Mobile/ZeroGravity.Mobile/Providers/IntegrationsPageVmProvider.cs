using System;
using System.Collections.Generic;
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
    public class IntegrationsPageVmProvider : PageVmProviderBase, IIntegrationsPageVmProvider
    {
        private readonly IIntegrationDataService _integrationDataService;
        private readonly ILogger _logger;

        public IntegrationsPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IIntegrationDataService integrationDataService) : base(tokenService)
        {
            _integrationDataService = integrationDataService;
            _logger = loggerFactory?.CreateLogger<IntegrationsPageVmProvider>() ??
                      new NullLogger<IntegrationsPageVmProvider>();
        }

        public async Task<ApiCallResult<List<IntegrationDataProxy>>> GetIntegrationDataListAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _integrationDataService.GetIntegrationDataListAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                List<IntegrationDataProxy> integrationDataProxies = new List<IntegrationDataProxy>();

                foreach (var integrationDataDto in apiCallResult.Value)
                {
                    var integrationDataProxy = ProxyConverter.GetIntegrationDataProxy(integrationDataDto);

                    integrationDataProxies.Add(integrationDataProxy);
                }

                
                return ApiCallResult<List<IntegrationDataProxy>>.Ok(integrationDataProxies);
            }

            return ApiCallResult<List<IntegrationDataProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}