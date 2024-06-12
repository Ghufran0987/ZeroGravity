using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class SugarBeatSessionDataService : ISugarBeatSessionDataService
    {
        private readonly ILogger<SugarBeatSessionDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public SugarBeatSessionDataService(ILogger<SugarBeatSessionDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<SugarBeatSessionData> AddAsync(SugarBeatSessionData sessionData, bool saveChanges = true)
        {
            return await _repository.AddAsync(sessionData, saveChanges);
        }

        public async Task<SugarBeatSessionData> GetActiveSessionAsync(int accountId, DateTime targetDate, bool includeGlucoseData = false)
        {
            var sessionData = await _repository.Execute(new GetSugarBeatActiveSessionData(accountId, targetDate, includeGlucoseData));
            return sessionData;
        }

        public async Task<SugarBeatSessionData> GetByIdAsync(int id, bool includeGlucoseData = false)
        {
            var sessionData = await _repository.Execute(new GetSugarBeatSessionDataById(id, includeGlucoseData));
            return sessionData;
        }

        public async Task<bool> GetIsSessionWarmedUpAsync(int sessionId)
        {
            var sessionData = await _repository.Execute(new IsSessionWarmedUp(sessionId));
            return sessionData;
        }

        public async Task<List<SugarBeatSessionData>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool includeGlucoseData = false)
        {
            var sessionData = await _repository.Execute(new GetSugarBeatSessionForPeriodAsync(accountId, fromDate, toDate, includeGlucoseData));
            return sessionData;
        }

        public async Task<SugarBeatSessionData> UpdateAsync(SugarBeatSessionData sessionData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetSugarBeatSessionDataById(sessionData.Id));
            return await _repository.UpdateAsync(entityToUpdate, sessionData, saveChanges);
        }

        public async Task<int> GetLatestMetabolicScoreAsync(int accountId)
        {
            return await _repository.Execute(new GetSugarBeatLatestMetabolicScore(accountId));
        }
    }
}