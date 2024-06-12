using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Services
{
    public class GlucoseDataService : IGlucoseDataService
    {
        private readonly ILogger<GlucoseDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public GlucoseDataService(ILogger<GlucoseDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<GlucoseData> AddAsync(GlucoseData glucoseData, bool saveChanges = true)
        {
            return await _repository.AddAsync(glucoseData, saveChanges);
        }

        public async Task<GlucoseData> UpdateAsync(GlucoseData glucoseData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetGlucoseDataById(glucoseData.Id));

            return await _repository.UpdateAsync(entityToUpdate, glucoseData, saveChanges);
        }

        public async Task<GlucoseData> GetByIdAsync(int id, bool getSugarBeatData = false)
        {
            var glucoseData = await _repository.Execute(new GetGlucoseDataById(id, getSugarBeatData));

            return glucoseData;
        }

        public async Task<List<GlucoseData>> GetByAccountAndDateForAllAsync(int accountId, DateTime targetDate)
        {
            var glucoseDatas = await _repository.Execute(new GetGlucoseDatasByAccountIdAndDateForAll(accountId, targetDate));

            return glucoseDatas;
        }

        public async Task<List<GlucoseData>> GetByAccountAndDateForManualAsync(int accountId, DateTime targetDate)
        {
            var glucoseDatas = await _repository.Execute(new GetGlucoseDatasByAccountIdAndDateForManual(accountId, targetDate));

            return glucoseDatas;
        }

        public async Task<List<GlucoseData>> GetByAccountAndDateForSugarBeatAsync(int accountId, DateTime targetDate)
        {
            var glucoseDatas = await _repository.Execute(new GetGlucoseDatasByAccountIdAndDateForSugarBeat(accountId, targetDate));

            return glucoseDatas;
        }
    }
}
