using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Interfaces
{
    public interface IWeightTrackerDataService
    {
        Task<WeightTracker> AddAsync(WeightTracker weightTracker, bool saveChanges = true);

        Task<WeightData> AddWeightAsync(WeightData weightData, bool saveChanges = true);

        Task<WeightTracker> UpdateAsync(WeightTracker weightTracker, bool saveChanges = true);

        Task<WeightTracker> GetByIdAsync(int id);

        Task<WeightTracker> GetCurrentWeightTrackerAsync(int accountId);

        Task<List<WeightTracker>> GetAll(int accountId);
    }
}