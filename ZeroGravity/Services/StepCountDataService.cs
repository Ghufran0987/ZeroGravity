using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class StepCountDataService : IStepCountDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public StepCountDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<StepCountData> GetByIdAsync(int id)
        {
            var stepCountData = _repository.Execute(new GetStepCountDataById(id));

            return stepCountData;
        }

        public Task<StepCountData> GetByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var stepCountData = _repository.Execute(new GetStepCountDataByAccountId(accountId, dateTime));

            return stepCountData;
        }

        public async Task<StepCountData> AddAsync(StepCountData activityData, bool saveChanges = true)
        {
            return await _repository.AddAsync(activityData, saveChanges);
        }

        public async Task<StepCountData> UpdateAsync(StepCountData activityData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetStepCountDataById(activityData.Id));

            return await _repository.UpdateAsync(entityToUpdate, activityData, saveChanges);
        }
    }
}
