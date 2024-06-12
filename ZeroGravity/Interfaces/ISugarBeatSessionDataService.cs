using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Interfaces
{
    public interface ISugarBeatSessionDataService
    {
        Task<SugarBeatSessionData> AddAsync(SugarBeatSessionData glucoseData, bool saveChanges = true);

        Task<SugarBeatSessionData> UpdateAsync(SugarBeatSessionData glucoseData, bool saveChanges = true);

        Task<SugarBeatSessionData> GetByIdAsync(int id, bool includeGlucoseData = false);

        // Get Active Session: Search for Patch Started Alert up to 14 Hours from Now else null
        Task<SugarBeatSessionData> GetActiveSessionAsync(int accountId, DateTime targetDate, bool includeGlucoseData = false);

        Task<List<SugarBeatSessionData>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool includeGlucoseData = false);
        
        Task<int> GetLatestMetabolicScoreAsync(int accountId);

        Task<bool> GetIsSessionWarmedUpAsync(int sessionId);
    }
}