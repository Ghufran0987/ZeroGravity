using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class FastingSettingsService : IFastingSettingsService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public FastingSettingsService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<FastingSetting> GetByAccounIdAsync(int accountId)
        {
            var fastingSetting = _repository.Execute(new GetFastingSettingsByAccountId(accountId));

            return fastingSetting;
        }

        public async Task<FastingSetting> AddAsync(FastingSetting fastingSetting, bool saveChanges = true)
        {
            return await _repository.AddAsync(fastingSetting, saveChanges);
        }

        public async Task<FastingSetting> UpdateAsync(FastingSetting fastingSetting, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetFastingSettingsByAccountId(fastingSetting.AccountId));

            return await _repository.UpdateAsync(entityToUpdate, fastingSetting, saveChanges);
        }
    }
}