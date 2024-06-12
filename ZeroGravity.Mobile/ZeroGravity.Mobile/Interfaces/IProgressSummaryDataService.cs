using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IProgressSummaryDataService
    {
        Task<ApiCallResult<List<ProgressSummaryDto>>> GetAnalysisSummaryDataByDayAsync(DateTime dateTime, CancellationToken cancellationToken);

        Task<ApiCallResult<List<ProgressSummaryDto>>> GetAnalysisSummaryDataByPeriodAsync(DateTime fromTime, DateTime toTime, CancellationToken cancellationToken);
    }
}