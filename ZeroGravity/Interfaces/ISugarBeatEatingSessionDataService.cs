using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Interfaces
{
    public interface ISugarBeatEatingSessionDataService
    {
        Task<SugarBeatEatingSession> AddAsync(SugarBeatEatingSession glucoseData, bool saveChanges = true);

        Task<SugarBeatEatingSession> UpdateAsync(SugarBeatEatingSession glucoseData, bool saveChanges = true);

        Task<SugarBeatEatingSession> GetByIdAsync(int id);

        Task<List<SugarBeatEatingSession>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate);

        Task<SugarBeatEatingSession> GetSessionByDateAsync(int accountId, DateTime startDate);

        Task ComputeMetabolicScoreAsync(int id);
    }
}