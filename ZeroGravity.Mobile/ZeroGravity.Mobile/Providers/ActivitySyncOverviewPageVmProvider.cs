using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.Providers
{
    public class ActivitySyncOverviewPageVmProvider : PageVmProviderBase, IActivitySyncOverviewPageVmProvider
    {
        private readonly IIntegrationDataService _integrationDataService;
        private readonly IFitbitIntegrationService _fitbitIntegrationService;

        public ActivitySyncOverviewPageVmProvider(ITokenService tokenService, IIntegrationDataService integrationDataService, IFitbitIntegrationService fitbitIntegrationService) : base(tokenService)
        {
            _integrationDataService = integrationDataService;
            _fitbitIntegrationService = fitbitIntegrationService;
        }

        public async Task<ApiCallResult<List<IntegrationDataProxy>>> GetLinkedIntegrationListAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _integrationDataService.GetLinkedIntegrationListAsync(cancellationToken);

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

        public async Task<ApiCallResult<List<SyncActivityProxy>>> GetActivitySyncDataAsnyc(IntegrationDataProxy integrationDataProxy, DateTime targetDateTime, CancellationToken cancellationToken)
        {
            if (integrationDataProxy.Name.Equals(IntegrationNameConstants.Fitbit))
            {
                var apiCallResult = await _fitbitIntegrationService.GetFitbitActivitiesAsync(integrationDataProxy.Id, targetDateTime, cancellationToken);

                if (apiCallResult.Success)
                {
                    apiCallResult.Value.TargetDate = targetDateTime;
                    apiCallResult.Value.IntegrationId = integrationDataProxy.Id;

                    var syncActivities = ProxyConverter.GetFitbitActivityProxies(apiCallResult.Value);

                    return ApiCallResult<List<SyncActivityProxy>>.Ok(syncActivities);
                }

                
                return ApiCallResult<List<SyncActivityProxy>>.Error(apiCallResult.ErrorMessage);
            }


            return ApiCallResult<List<SyncActivityProxy>>.Error("");
        }
    }
}