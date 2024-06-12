using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Queries.GlucoseData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class EducationalInfoDataService : IEducationalInfoDataService
    {
        private readonly ILogger<EducationalInfoDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public EducationalInfoDataService(ILogger<EducationalInfoDataService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<EducationalInfoData> AddAsync(EducationalInfoData educationalInfoData, bool saveChanges = true)
        {
            return await _repository.AddAsync(educationalInfoData, saveChanges);
        }

        public async Task<EducationalInfoData> GetRandomByCategoryAsync(string category)
        {
            var glucoseData = await _repository.Execute(new GetRandomByCategory(category));
            return glucoseData;
        }

        public async Task<EducationalInfoData> GetByIdAsync(int id)
        {
            return await _repository.Execute(new GetEducationalInfoDataById(id));
        }

        public async Task<EducationalInfoData> UpdateAsync(EducationalInfoData educationalInfoData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetEducationalInfoDataById(educationalInfoData.Id));
            return await _repository.UpdateAsync(entityToUpdate, educationalInfoData, saveChanges);
        }
    }
}