using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMealsSnacksPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<MealsBadgeInformationProxy>> GetMealsBadgeInformationAsnyc(DateTime targetDateTime,
            CancellationToken cancellationToken);
    }
}