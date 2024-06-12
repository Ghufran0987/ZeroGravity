using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWellbeingDataPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<WellbeingDataProxy>> GetDayToDayActivityAsnyc(int activityId,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataProxy>> GetWellbeingDataByDateAsnyc(DateTime dateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataProxy>> CreatWellbeingDataAsnyc(WellbeingDataProxy wellbeingDataProxy,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataProxy>> UpdateWellbeingDataAsnyc(WellbeingDataProxy wellbeingDataProxy,
            CancellationToken cancellationToken);
    }
}