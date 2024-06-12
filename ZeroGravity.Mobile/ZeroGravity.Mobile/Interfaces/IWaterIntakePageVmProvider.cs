using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWaterIntakePageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataAsnyc(int activityId, CancellationToken cancellationToken);
        Task<ApiCallResult<double>> GetLiquidIntakeDataSummaryAsync(DateTime dateTime, LiquidType type, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeDataProxy>> CreateLiquidIntakeDataAsnyc(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeDataProxy>> UpdateLiquidIntakeDataAsnyc(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken);
    }
}