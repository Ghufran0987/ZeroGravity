using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class DietPrefrerenceService : IDietPreferenceService
    {
        private readonly ILogger<DietPrefrerenceService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public DietPrefrerenceService(ILogger<DietPrefrerenceService> logger,
            IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        //MedicalCondition functions
        public Task<DietPreference> GetByAccounId(int accountId)
        {
            var dietPreference = _repository.Execute(new GetDietPreferencesByAccountId(accountId));

            return dietPreference;
        }

        public async Task<DietPreference> AddAsync(DietPreference medicalCondition, bool saveChanges = true)
        {
            return await _repository.AddAsync(medicalCondition, saveChanges);
        }

        public async Task<DietPreference> UpdateAsync(DietPreference dietPreference, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetDietPreferencesByAccountId(dietPreference.AccountId));

            dietPreference.Id = entityToUpdate.Id;

            return await _repository.UpdateAsync(entityToUpdate, dietPreference, saveChanges);
        }
    }
}