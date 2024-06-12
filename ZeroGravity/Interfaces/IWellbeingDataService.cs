using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IWellbeingDataService
    {
        Task<WellbeingData> GetByIdAsync(int id);

        Task<WellbeingData> GetByAccountIdAndDateAsync(int accountId, DateTime dateTime);

        Task<WellbeingData> AddAsync(WellbeingData wellbeingData, bool saveChanges = true);

        Task<WellbeingData> UpdateAsync(WellbeingData wellbeingData, bool saveChanges = true);

        Task<List<WellbeingData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime);
        Task<List<WellbeingData>> GetAllByAccountIdAndDateRangeAsync(int accountId, DateTime dateFrom, DateTime dateTo);
    }
}