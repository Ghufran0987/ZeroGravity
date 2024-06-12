using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IBadgeInformationService
    {
        Task<ApiCallResult<ActivityBadgeInformationDto>> GetActivityBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<LiquidIntakeBadgeInformationDto>> GetLiquidIntakeBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<MealsBadgeInformationDto>> GetMealsBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<MyDayBadgeInformationDto>> GetMyDayBadgeInformationByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
    }
}
