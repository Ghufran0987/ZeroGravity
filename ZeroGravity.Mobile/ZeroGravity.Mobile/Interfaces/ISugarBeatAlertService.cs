using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
   public interface ISugarBeatAlertService
    {
        Task<ApiCallResult<SugarBeatAlertDto>> AddAsync(SugarBeatAlertDto alertData, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatAlertDto>> UpdateAsync(SugarBeatAlertDto alertData, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatAlertDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ApiCallResult<List<SugarBeatAlertDto>>> GetAlertForPeriodAsync( DateTime fromDate, DateTime toDate, AlertCode? alertCode, CRCCodes? criticalCode, CancellationToken cancellationToken);
    }
}
