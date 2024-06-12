using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IGlucoseDataService
    {
        Task<GlucoseData> AddAsync(GlucoseData glucoseData, bool saveChanges = true);
        Task<GlucoseData> UpdateAsync(GlucoseData glucoseData, bool saveChanges = true);

        Task<GlucoseData> GetByIdAsync(int id, bool getSugarBeatData);

        Task<List<GlucoseData>> GetByAccountAndDateForAllAsync(int accountId, DateTime targetDate);
        Task<List<GlucoseData>> GetByAccountAndDateForManualAsync(int accountId, DateTime targetDate);
        Task<List<GlucoseData>> GetByAccountAndDateForSugarBeatAsync(int accountId, DateTime targetDate);
    }
}
