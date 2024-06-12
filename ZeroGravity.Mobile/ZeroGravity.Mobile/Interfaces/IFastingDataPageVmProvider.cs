using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFastingDataPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<FastingDataProxy>> GetFastingDataByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingDataProxy>> CreateFastingDataAsnyc(FastingDataProxy fastingDataProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingDataProxy>> UpdateFastingDataAsync(FastingDataProxy fastingDataProxy, CancellationToken cancellationToken);
    }
}