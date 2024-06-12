using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IStepCountDataService
    {
        Task<ApiCallResult<StepCountDataDto>> GetStepCountDataAsync(int activityId, CancellationToken cancellationToken);
        Task<ApiCallResult<StepCountDataDto>> GetStepCountDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<StepCountDataDto>> CreateStepCountDataAsync(StepCountDataDto activityDataDto, CancellationToken cancellationToken);
        Task<ApiCallResult<StepCountDataDto>> UpdateStepCountDataAsync(StepCountDataDto stepCountDataDto, CancellationToken cancellationToken);
    }
}
