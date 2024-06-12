using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityData> GetByIdAsync(int id);

        Task<List<ActivityData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime);
        Task<List<ActivityData>> GetAllByAccountIdAndDateRangeAsync(int accountId, DateTime dateFrom, DateTime dateTo);

        Task<ActivityData> AddAsync(ActivityData activityData, bool saveChanges = true);

        Task<ActivityData> UpdateAsync(ActivityData activityData, bool saveChanges = true);

        Task<ActivityData> ImportActivityAsync(ActivityData activityData, bool saveChanges = true);

        Task ComputeNewActivityGoalAsync(int id);
    }
}