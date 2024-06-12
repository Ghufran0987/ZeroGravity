using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Services
{
    public class LiquidIntakeService : ILiquidIntakeService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public LiquidIntakeService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<List<LiquidIntake>> GetByAccoundAndDateAsync(int accountId, DateTime targetDate)
        {
            var liquidIntakes = await _repository.Execute(new GetLiquidIntakesByDate(accountId, targetDate));
            return liquidIntakes;
        }

        public async Task<List<LiquidIntake>> GetByAccountAndDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var liquidIntakes = await _repository.Execute(new GetLiquidIntakeFromDatespan(accountId, fromDate, toDate));
            return liquidIntakes;
        }

        public async Task<List<LiquidIntake>> GetByAccountAndDateAndTypeAsync(int accountId, DateTime targetDate, LiquidType type)
        {
            return await _repository.Execute(new GetLiquidIntakesByDateAndType(accountId, targetDate, type));
        }

        public Task<LiquidIntake> GetByIdAsync(int id)
        {
            var liquidIntake = _repository.Execute(new GetLiquidIntakeById(id));

            return liquidIntake;
        }

        public async Task<LiquidIntake> AddAsync(LiquidIntake liquidIntake, bool saveChanges = true)
        {
            return await _repository.AddAsync(liquidIntake, saveChanges);
        }

        public async Task<LiquidIntake> UpdateAsync(LiquidIntake liquidIntake, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetLiquidIntakeById(liquidIntake.Id));

            return await _repository.UpdateAsync(entityToUpdate, liquidIntake, saveChanges);
        }
    }
}