using System;
using System.Threading.Tasks;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackSummaryDto> GetFeedbackByAccountIdAndDateAsync(int accountId, DateTime dateTime);
    }
}