using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISugarBeatGlucoseService
    {
        Task<ApiCallResult<SugarBeatGlucoseDto>> AddAsync(SugarBeatGlucoseDto alertData, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatGlucoseDto>> UpdateAsync(SugarBeatGlucoseDto alertData, CancellationToken cancellationToken, bool saveChanges = true );

        Task<ApiCallResult<List<SugarBeatGlucoseDto>>> GetGlucoseForPeriodAsync( DateTime fromDate, DateTime toDate, CancellationToken cancellationToken, bool isGlucoseNull = false);

        Task<ApiCallResult<List<SugarBeatGlucoseDto>>> GetAllGlucoseForSessionId( int sessionId, CancellationToken cancellationToken, bool isGlucoseNull = false);
    }
}
