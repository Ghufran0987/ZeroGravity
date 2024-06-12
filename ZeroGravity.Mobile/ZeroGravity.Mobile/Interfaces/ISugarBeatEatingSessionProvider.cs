using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISugarBeatEatingSessionProvider : IPageVmProvider
    {
        Task<ApiCallResult<SugarBeatEatingSessionProxy>> AddSugarBeatEatingSessionAsync(SugarBeatEatingSessionProxy proxy, CancellationToken cancellationToken);
        Task<ApiCallResult<SugarBeatEatingSessionProxy>> GetSugarBeatEatingSessionAsync(int id, CancellationToken cancellationToken);
        Task<ApiCallResult<IEnumerable<SugarBeatEatingSessionProxy>>> GetSugarBeatEatingSessionsAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
   
        Task<ApiCallResult<SugarBeatEatingSessionProxy>> UpdateSugarBeatEatingSessionAsync(SugarBeatEatingSessionProxy proxy, CancellationToken cancellationToken);
        Task<ApiCallResult<bool>> IsSenorWarmedUp(int sessionId, CancellationToken cancellationToken);
    }
}