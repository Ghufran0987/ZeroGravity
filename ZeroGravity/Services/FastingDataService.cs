using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class FastingDataService : IFastingDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public FastingDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<FastingData> GetByAccountIdAsync(int accountId, DateTime targetDate)
        {
            return await Task.Run(() =>
            {
                var fastingData = _repository.Execute(new GetFastingDataByAccountIdAndDate(accountId, targetDate));
                return fastingData;
            });
        }

        public async Task<List<FastingData>> GetByAccountIdAndDateRangeAsync(int accountId, DateTime dateStart, DateTime dateEnd)
        {
            return await Task.Run(() =>
            {
                var fastingData = _repository.Execute(new GetFastingDataByAccountIdAndDateRange(accountId, dateStart, dateEnd));
                return fastingData;
            });
        }

        public async Task<IEnumerable<FastingData>> GetActivesByAccountIdAsync(int accountId, DateTime targetDate)
        {
            return await Task.Run(() =>
            {
                var fastingDataList =
                    _repository.Execute(new GetActiveFastingDataByAccountIdAndDate(accountId, targetDate));

                return fastingDataList;
            });
        }

        public async Task<FastingData> AddAsync(FastingData fastingSetting, bool saveChanges = true)
        {
            return await _repository.AddAsync(fastingSetting, saveChanges);
        }

        public async Task<FastingData> UpdateAsync(FastingData fastingSetting, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetFastingDataByAccountIdAndDate(fastingSetting.AccountId, fastingSetting.Created));

            return await _repository.UpdateAsync(entityToUpdate, fastingSetting, saveChanges);
        }
    }
}