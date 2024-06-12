using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
   public interface ISugarBeatSessionService
    {
        Task<ApiCallResult<SugarBeatSessionDto>> AddAsync(SugarBeatSessionDto sessionData, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatSessionDto>> UpdateAsync(SugarBeatSessionDto sessionData, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatSessionDto>> GetActiveSessionAsync(DateTime targetDate, CancellationToken cancellationToken, bool includeGlucoseData = false);

        Task<ApiCallResult<SugarBeatSessionDto>> GetByIdAsync(int id, CancellationToken cancellationToken, bool includeGlucoseData = false);

        Task<ApiCallResult<List<SugarBeatSessionDto>>> GetSessionForPeriodAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken, bool includeGlucoseData = false);
    }
}
