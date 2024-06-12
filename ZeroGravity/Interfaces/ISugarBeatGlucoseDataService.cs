using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Interfaces
{
    public interface ISugarBeatGlucoseDataService
    {
        Task<SugarBeatGlucoseData> AddAsync(SugarBeatGlucoseData glucoseData, bool saveChanges = true);

        Task<List<SugarBeatGlucoseData>> GetGlucoseForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool isGlucoseNull = false);

        Task<List<SugarBeatGlucoseData>> GetAllGlucoseForSessionId(int accountId, int sessionId, bool isGlucoseNull = false);

        Task<SugarBeatGlucoseData> UpdateAsync(SugarBeatGlucoseData glucoseData, bool saveChanges = true);

        Task<SugarBeatGlucoseData> GetByIdAsync(int id);

        // Sync status table and Session Table
    }
}