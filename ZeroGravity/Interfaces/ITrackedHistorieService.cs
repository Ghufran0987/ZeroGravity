using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface ITrackedHistorieService
    {
        Task<List<TrackedHistoryDto>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime);
    }
}
