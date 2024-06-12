using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAnalysisSummaryDataService
    {
        Task<ApiCallResult<AnalysisSummaryDto>> GetAnalysisSummaryDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken);
    }
}