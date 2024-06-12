using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IActivitySyncOverviewPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<List<IntegrationDataProxy>>> GetLinkedIntegrationListAsnyc(
            CancellationToken cancellationToken);

        Task<ApiCallResult<List<SyncActivityProxy>>> GetActivitySyncDataAsnyc(
            IntegrationDataProxy integrationDataProxy, DateTime targetDateTime, CancellationToken cancellationToken);
    }
}