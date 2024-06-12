using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMealsSnacksUnhealthySnackPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<MealDataProxy>> AddUnhealthySnackAsync(MealDataProxy proxy, CancellationToken cancellationToken);
        Task<ApiCallResult<IEnumerable<FastingDataProxy>>> GetActiveFastingDataAsync(DateTime dateTime, CancellationToken cancellationToken);
    }
}