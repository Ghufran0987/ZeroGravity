using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class SugarBeatSettingDataService : ISugarBeatSettingDataService
    {
        private readonly ILogger<SugarBeatSettingDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public SugarBeatSettingDataService(ILogger<SugarBeatSettingDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<SugarBeatSettingData> AddAsync(SugarBeatSettingData sessionData, bool saveChanges = true)
        {
            // TODO Convert to UTC before Save
            return await _repository.AddAsync(sessionData, saveChanges);
        }

        public async Task<SugarBeatSettingData> GetByAccountIdAsync(int accountId)
        {
            return await _repository.Execute(new GetSugarBeatSettingDataByAccountId(accountId));
        }

        public async Task<SugarBeatSettingData> UpdateAsync(SugarBeatSettingData settingData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetSugarBeatSettingDataByAccountId(settingData.AccountId));
            return await _repository.UpdateAsync(entityToUpdate, settingData, saveChanges);
        }
    }
}