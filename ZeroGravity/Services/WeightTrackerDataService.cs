using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries.Weight;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class WeightTrackerDataService : IWeightTrackerDataService
    {
        private readonly ILogger<WeightTrackerDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public WeightTrackerDataService(ILogger<WeightTrackerDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<WeightTracker> AddAsync(WeightTracker weightTracker, bool saveChanges = true)
        {
            // Check for duplicate
            var found = await _repository.Execute(new GetWeightTrackerByDateRange(weightTracker.AccountId, weightTracker));
            if (found) return null;
            var result = await _repository.AddAsync(weightTracker, saveChanges);
            return result;
        }

        public async Task<WeightData> AddWeightAsync(WeightData weightData, bool saveChanges = true)
        {
            // Check for duplicate
            var found = await _repository.Execute(new GetWeightDataByDate(weightData.AccountId, weightData));
            if (found != null)
            {
                // Update
                found.Value = weightData.Value;
                return await _repository.UpdateAsync(found, saveChanges);
            }
            else
            {
                var result = await _repository.AddAsync(weightData, saveChanges);
                return result;
            }
        }

        public async Task<List<WeightTracker>> GetAll(int accountId)
        {
            var data = await _repository.Execute(new GetAllWeightTracker(accountId));
            return data;
        }

        public async Task<WeightTracker> GetByIdAsync(int id)
        {
            var data = await _repository.Execute(new GetWeightTrackerDataById(id));
            return data;
        }

        public async Task<WeightTracker> GetCurrentWeightTrackerAsync(int accountId)
        {
            var data = await _repository.Execute(new GetCurrentWeightTrackerByAccountId(accountId));
            return data;
        }

        public async Task<WeightTracker> UpdateAsync(WeightTracker weightTracker, bool saveChanges = true)
        {
            return await _repository.UpdateAsync(weightTracker, saveChanges);
        }
    }
}