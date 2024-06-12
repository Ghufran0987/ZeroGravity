using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
  public  interface ISugarBeatEatingSessionService
    {
        Task<ApiCallResult<SugarBeatEatingSessionDto>> AddAsync(SugarBeatEatingSessionDto alertDto, CancellationToken cancellationToken, bool saveChanges = true);
        Task<ApiCallResult<List<SugarBeatEatingSessionDto>>> GetSugarBeatEatingSessionForPeriodAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
        Task<ApiCallResult<SugarBeatEatingSessionDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiCallResult<SugarBeatEatingSessionDto>> UpdateAsync(SugarBeatEatingSessionDto alertDto, CancellationToken cancellationToken, bool saveChanges = true);
        Task<ApiCallResult<bool>> GetIsSensorWarmedUp(int sessionId, CancellationToken cancellationToken);
    }
}
