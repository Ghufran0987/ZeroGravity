using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ICalorieDrinksAlcoholPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataAsync(int activityId, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeDataProxy>> CreateLiquidIntakeDataAsync(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeDataProxy>> UpdateLiquidIntakeDataAsync(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken);

        Task<ApiCallResult<IEnumerable<FastingDataProxy>>> GetActiveFastingDataAsync(DateTime dateTime, CancellationToken cancellationToken);
    }
}