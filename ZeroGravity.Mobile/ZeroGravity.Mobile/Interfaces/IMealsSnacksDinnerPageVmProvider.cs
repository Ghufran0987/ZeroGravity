using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMealsSnacksDinnerPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<MealDataProxy>> AddDinnerAsync(MealDataProxy proxy, CancellationToken cancellationToken);
        Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<IEnumerable<FastingDataProxy>>> GetActiveFastingDataAsync(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<List<MealDataProxy>>> GetMealDataWithFilterAsync(FilterMealDataDto dto, CancellationToken cancellationToken);
        Task<ApiCallResult<MealDataProxy>> UpdateDinnerAsync(MealDataProxy proxy, CancellationToken cancellationToken);
    }
}