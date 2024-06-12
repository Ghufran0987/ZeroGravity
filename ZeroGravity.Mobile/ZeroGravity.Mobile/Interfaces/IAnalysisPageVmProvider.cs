using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAnalysisPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<List<AnalysisItemProxy>>> GetAnalysisSummaryDataByDateAsync(DateTime dateTime,
            CancellationToken cancellationToken);

         Task<ApiCallResult<ProgressProxy>> GetProgressDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken,string progressType);
    }
}