using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IStepCountDataService
    {
        Task<StepCountData> GetByIdAsync(int id);
        Task<StepCountData> GetByAccountIdAndDateAsync(int accountId, DateTime dateTime);
        Task<StepCountData> AddAsync(StepCountData activityData, bool saveChanges = true);
        Task<StepCountData> UpdateAsync(StepCountData activityData, bool saveChanges = true);
    }
}
