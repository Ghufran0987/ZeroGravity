using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ITrackedHistoryService
    {
        Task<ApiCallResult<List<TrackedHistoryDto>>> GetTrackedHistoryByDateAsync(DateTime targetDateTime, CancellationToken cancellationToken);
    }
}
