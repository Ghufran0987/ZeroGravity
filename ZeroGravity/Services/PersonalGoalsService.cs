using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class PersonalGoalsService : IPersonalGoalsService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public PersonalGoalsService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<PersonalGoal> GetByAccounIdAsync(int accountId)
        {
            var personalGoal = _repository.Execute(new GetPersonalGoalsByAccountId(accountId));

            return personalGoal;
        }

        public async Task<PersonalGoal> AddAsync(PersonalGoal personalGoal, bool saveChanges = true)
        {
            return await _repository.AddAsync(personalGoal, saveChanges);
        }

        public async Task<PersonalGoal> UpdateAsync(PersonalGoal personalGoal, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetPersonalGoalsByAccountId(personalGoal.AccountId));

            personalGoal.Id = entityToUpdate.Id;

            return await _repository.UpdateAsync(entityToUpdate, personalGoal, saveChanges);
        }
    }
}