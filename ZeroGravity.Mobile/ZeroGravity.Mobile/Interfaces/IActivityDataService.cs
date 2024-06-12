using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IActivityDataService
    {
        Task<ApiCallResult<ActivityDataDto>> GetActivityDataAsync(int activityId, CancellationToken cancellationToken);

        Task<ApiCallResult<ActivityDataDto>> GetActivityDataByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<ActivityDataDto>> CreateActivityDataAsync(ActivityDataDto activityDataDto,
            CancellationToken cancellationToken);

        Task<ApiCallResult<ActivityDataDto>> UpdateActivityDataAsync(ActivityDataDto activityDataDto,
            CancellationToken cancellationToken);

        Task<ApiCallResult<ActivityDataDto>> SyncActivityDataAsync(ActivityDataDto activityDataDto,
            CancellationToken cancellationToken);

        Task<ApiCallResult<List<ActivityDataDto>>> GetActivitiesByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken);
    }
}