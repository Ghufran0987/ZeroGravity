using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Interfaces
{
    public interface ISugarBeatAlertDataService
    {
        Task<SugarBeatAlertData> AddAsync(SugarBeatAlertData glucoseData, bool saveChanges = true);

        Task<SugarBeatAlertData> UpdateAsync(SugarBeatAlertData glucoseData, bool saveChanges = true);

        Task<SugarBeatAlertData> GetByIdAsync(int id);

        Task<List<SugarBeatAlertData>> GetAlertForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, AlertCode? alertCode, CRCCodes? criticalCode);
    }
}