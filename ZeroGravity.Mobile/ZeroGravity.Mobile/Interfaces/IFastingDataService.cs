using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFastingDataService
    {
        Task<ApiCallResult<FastingDataDto>> GetFastingDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<IEnumerable<FastingDataDto>>> GetActiveFastingDataByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingDataDto>> CreateFastingDataAsync(FastingDataDto fastingDataDto, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingDataDto>> UpdateFastingDataAsync(FastingDataDto fastingSettingDto, CancellationToken cancellationToken);
    }
}
