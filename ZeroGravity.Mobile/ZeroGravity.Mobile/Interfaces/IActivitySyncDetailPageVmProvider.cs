using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IActivitySyncDetailPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<List<SyncActivityProxy>>> SynchroniseActivitiesAsnyc(
            List<SyncActivityProxy> syncActivityProxies, CancellationToken cancellationToken);
    }
}