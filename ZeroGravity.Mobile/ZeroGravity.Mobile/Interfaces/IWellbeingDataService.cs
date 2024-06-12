using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWellbeingDataService
    {
        Task<ApiCallResult<WellbeingDataDto>> GetWellbeingDataAsync(int activityId,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataDto>> GetWellbeingDataByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataDto>> CreateWellbeingDataAsync(WellbeingDataDto activityDataDto,
            CancellationToken cancellationToken);

        Task<ApiCallResult<WellbeingDataDto>> UpdateWellbeingDataAsync(WellbeingDataDto activityDataDto,
            CancellationToken cancellationToken);
    }
}
