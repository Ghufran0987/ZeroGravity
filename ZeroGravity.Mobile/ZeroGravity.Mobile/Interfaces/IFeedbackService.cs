using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFeedbackService
    {
        Task<ApiCallResult<FeedbackSummaryDto>> GetFeedbackSummaryDtoByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken);

        FeedbackDataProxy SetFeedbackState(FeedbackDataProxy feedbackDataProxy);
    }
}