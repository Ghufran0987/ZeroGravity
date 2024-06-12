using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMeditationAreaPageVmProvider : IPageVmProvider
    {
        Task SaveTimeInLocalStorageAsync(TimeSpan t);
        Task<TimeSpan> LoadTimeFromLocalStorageAsync();
        Task RemoveTimeFromLocalStorageAsync();

        Task<ApiCallResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(
            CancellationToken token = new CancellationToken());

        Task<ApiCallResult<MeditationDataProxy>> SaveMeditationDataAsync(MeditationDataProxy meditationDataProxy, CancellationToken cancellationToken = new CancellationToken());
        Task<ApiCallResult<TimeSpan>> GetMeditationDurationForDateAsync(DateTime date, CancellationToken cancellationToken);
    }
}
