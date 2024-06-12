using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class ActivitySyncDetailPageVmProvider : PageVmProviderBase, IActivitySyncDetailPageVmProvider
    {
        private readonly IActivityDataService _activityDataService;

        public ActivitySyncDetailPageVmProvider(ITokenService tokenService, IActivityDataService activityDataService) : base(tokenService)
        {
            _activityDataService = activityDataService;
        }

        public async Task<ApiCallResult<List<SyncActivityProxy>>> SynchroniseActivitiesAsnyc(List<SyncActivityProxy> syncActivityProxies, CancellationToken cancellationToken)
        {
            foreach (var syncActivityProxy in syncActivityProxies)
            {
                var activityDataDto = ProxyConverter.GetActivityDataDto(syncActivityProxy);

                var apiCallResult = await _activityDataService.SyncActivityDataAsync(activityDataDto, cancellationToken);

                if (apiCallResult.Success)
                {
                    syncActivityProxy.Id = apiCallResult.Value.Id;
                    syncActivityProxy.AccountId = apiCallResult.Value.AccountId;
                }
                else
                {
                    return ApiCallResult<List<SyncActivityProxy>>.Error(apiCallResult.ErrorMessage);
                }
            }

            return ApiCallResult<List<SyncActivityProxy>>.Ok(syncActivityProxies);
        }
    }
}