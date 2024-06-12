using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class MedicalConditionService : IMedicalConditionService
    {
        private readonly ILogger<MedicalConditionService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public MedicalConditionService(ILogger<MedicalConditionService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        //MedicalCondition functions
        public Task<MedicalCondition> GetByAccounId(int accountId)
        {
            var medicalCondition = _repository.Execute(new GetMedicalConditionByAccountId(accountId));

            return medicalCondition;
        }

        public async Task<MedicalCondition> AddAsync(MedicalCondition medicalCondition, bool saveChanges = true)
        {
            return await _repository.AddAsync(medicalCondition, saveChanges);
        }

        public async Task<MedicalCondition> UpdateAsync(MedicalCondition medicalCondition, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetMedicalConditionByAccountId(medicalCondition.AccountId));

            medicalCondition.Id = entityToUpdate.Id;

            return await _repository.UpdateAsync(entityToUpdate, medicalCondition, saveChanges);
        }
    }
}