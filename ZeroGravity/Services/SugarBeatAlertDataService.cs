using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Services
{
    public class SugarBeatAlertDataService : ISugarBeatAlertDataService
    {
        private readonly ILogger<SugarBeatAlertDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public SugarBeatAlertDataService(ILogger<SugarBeatAlertDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<SugarBeatAlertData> AddAsync(SugarBeatAlertData alertData, bool saveChanges = true)
        {
            // Check for duplicate
            SugarBeatAlertData alert = await _repository.Execute(new GetSugarBeatAlertDataByDate(alertData.AccountId, alertData.Created, alertData.TransmitterId, alertData.Code));
            if (alert != null)
            {
                return alert;
            }
            return await _repository.AddAsync(alertData, saveChanges);
        }

        public async Task<List<SugarBeatAlertData>> GetAlertForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, AlertCode? alertCode, CRCCodes? criticalCode)
        {
            var glucoseData = await _repository.Execute(new GetSugarBeatAlertDataForPeriod(accountId, fromDate, toDate, alertCode, criticalCode));
            return glucoseData;
        }

        public async Task<SugarBeatAlertData> GetByIdAsync(int id)
        {
            return await _repository.Execute(new GetSugarBeatAlertDataById(id));
        }

        public async Task<SugarBeatAlertData> UpdateAsync(SugarBeatAlertData alertData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetSugarBeatAlertDataById(alertData.Id));
            return await _repository.UpdateAsync(entityToUpdate, alertData, saveChanges);
        }
    }
}