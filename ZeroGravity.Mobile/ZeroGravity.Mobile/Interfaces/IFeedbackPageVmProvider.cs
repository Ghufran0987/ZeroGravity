using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFeedbackPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<FeedbackSummaryProxy>> GetFeedbackSummaryAsync(DateTime targetDateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<AccountResponseDto>> GetAccountDetailsAsync(CancellationToken cancellationToken);

    }
}