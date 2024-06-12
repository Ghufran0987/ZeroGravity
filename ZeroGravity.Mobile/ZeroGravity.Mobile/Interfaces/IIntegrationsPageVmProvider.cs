using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IIntegrationsPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<List<IntegrationDataProxy>>> GetIntegrationDataListAsnyc(
            CancellationToken cancellationToken);
    }
}