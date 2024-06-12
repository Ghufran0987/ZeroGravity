using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class PersonalDataService : IPersonalDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public PersonalDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<PersonalData> GetByAccounIdAsync(int accountId)
        {
            var personalData = await _repository.Execute(new GetPersonalDataByAccountId(accountId));

            return personalData;
        }

        public async Task<PersonalData> AddAsync(PersonalData personalData, bool saveChanges = true)
        {
            return await _repository.AddAsync(personalData, saveChanges);
        }

        public async Task<PersonalData> UpdateAsync(PersonalData personalData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetPersonalDataByAccountId(personalData.AccountId));

            personalData.Id = entityToUpdate.Id;

            entityToUpdate.AccountId = personalData.AccountId;
            entityToUpdate.Country = personalData.Country;
            entityToUpdate.DeviceType = personalData.DeviceType;
            entityToUpdate.Ethnicity = personalData.Ethnicity;
            entityToUpdate.BiologicalGender = personalData.BiologicalGender;
            entityToUpdate.IdentifyGender = personalData.IdentifyGender;
            entityToUpdate.Height = personalData.Height;
            entityToUpdate.HipDiameter = personalData.HipDiameter;
            entityToUpdate.NeckDiameter = personalData.NeckDiameter;
            entityToUpdate.Salutation = personalData.Salutation;
            entityToUpdate.TimeZone = personalData.TimeZone;
            entityToUpdate.WaistDiameter = personalData.WaistDiameter;
            entityToUpdate.Weight = personalData.Weight;
            entityToUpdate.DateOfBirth = personalData.DateOfBirth;

            // Delete all questions and replace with new anwsers
            var list = entityToUpdate.QuestionAndAnswers.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                await _repository.DeleteAsync(list[i], false);
            }

            // Write new answers
            foreach (QuestionAndAnswerData newQaA in personalData.QuestionAndAnswers)
            {
                newQaA.PersonalDataId = personalData.Id;
                await _repository.AddAsync(newQaA, false);
            }
            await _repository.Save();
            var result = await _repository.Execute(new GetPersonalDataByAccountId(personalData.AccountId));
            return result;
        }
    }
}