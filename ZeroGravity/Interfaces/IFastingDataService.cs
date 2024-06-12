using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IFastingDataService
    {
        Task<FastingData> GetByAccountIdAsync(int accountId, DateTime targetDate);
        Task<List<FastingData>> GetByAccountIdAndDateRangeAsync(int accountId, DateTime dateStart, DateTime dateEnd);
        Task<IEnumerable<FastingData>> GetActivesByAccountIdAsync(int accountId, DateTime targetDate);
        Task<FastingData> AddAsync(FastingData fastingSetting, bool saveChanges = true);
        Task<FastingData> UpdateAsync(FastingData fastingSetting, bool saveChanges = true);
    }
}
