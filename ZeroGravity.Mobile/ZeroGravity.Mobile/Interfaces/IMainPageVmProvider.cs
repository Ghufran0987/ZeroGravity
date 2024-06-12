using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMainPageVmProvider : IPageVmProvider
    {
        //Task<LoginResult> TrySilentLogin();

        Task<ApiCallResult<MyDayBadgeInformationProxy>> GetMyDayBadgeInformationAsnyc(DateTime targetDateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<List<TrackedHistoryProxy>>> GetTrackedHistoryAsnyc(DateTime targetDateTime,
            CancellationToken cancellationToken);
        bool CheckSugarbeatConnected();

        Task<SugarBeatAccessDetailsProxy> GetSugarBeatAccessDetailsAsync();
      
    }
}