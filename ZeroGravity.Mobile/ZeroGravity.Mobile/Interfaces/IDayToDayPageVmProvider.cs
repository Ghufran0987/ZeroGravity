using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IDayToDayPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<DayToDayActivityProxy>> GetDayToDayActivityAsnyc(int activityId, CancellationToken cancellationToken);
        Task<ApiCallResult<DayToDayActivityProxy>> GetDayToDayActivityByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<DayToDayActivityProxy>> CreatDayToDayActivityAsnyc(DayToDayActivityProxy dayToDayActivityProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<DayToDayActivityProxy>> UpdateDayToDayActivityAsnyc(DayToDayActivityProxy dayToDayActivityProxy, CancellationToken cancellationToken);
    }
}