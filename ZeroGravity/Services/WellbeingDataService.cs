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
    public class WellbeingDataService : IWellbeingDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public WellbeingDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<WellbeingData> GetByIdAsync(int id)
        {
            var wellbeingData = _repository.Execute(new GetWellbeingDataById(id));

            return wellbeingData;
        }

        public Task<WellbeingData> GetByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var wellBeingData = _repository.Execute(new GetWellbeingDataByAccountId(accountId, dateTime));

            return wellBeingData;
        }

        public Task<List<WellbeingData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var wellBeingData = _repository.Execute(new GetAllWellbeingDataByAccountId(accountId, dateTime));
            return wellBeingData;
        }

        public Task<List<WellbeingData>> GetAllByAccountIdAndDateRangeAsync(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            var wellBeingData = _repository.Execute(new GetAllWellbeingDataByAccountIdAndDateRange(accountId, dateFrom, dateTo));
            return wellBeingData;
        }

        public async Task<WellbeingData> AddAsync(WellbeingData wellbeingData, bool saveChanges = true)
        {
            return await _repository.AddAsync(wellbeingData, saveChanges);
        }

        public async Task<WellbeingData> UpdateAsync(WellbeingData wellbeingData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetWellbeingDataById(wellbeingData.Id));

            return await _repository.UpdateAsync(entityToUpdate, wellbeingData, saveChanges);
        }
    }
}