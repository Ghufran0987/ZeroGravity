using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ICoachingDetailPageVmProvider : IPageVmProvider
    {
        Task<string> GetCurrentUserEmailAsync(CancellationToken token);

        Task<bool> SubmitInterestAsync(int userId, string email, CoachingType type, List<string> options, CancellationToken token);
    }
}