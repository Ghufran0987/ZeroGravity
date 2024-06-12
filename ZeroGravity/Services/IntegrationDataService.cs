using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class IntegrationDataService : IIntegrationDataService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public IntegrationDataService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<List<IntegrationData>> GetAvailableIntegrationsAsync()
        {
            var integrationDatas = await _repository.Execute(new GetSupportedIntegrations());

            return integrationDatas;
        }
        public async Task<IntegrationData> GetIntegrationByNameAsync(string integrationName)
        {
            var integrationData = await _repository.Execute(new GetIntegrationByName(integrationName));

            return integrationData;
        }

        public Task<List<LinkedIntegration>> GetLinkedIntegrationsByAccountIdAsync(int accountId)
        {
            var linkedIntegrations = _repository.Execute(new GetLinkedIntegrationsByAccountId(accountId));

            return linkedIntegrations;
        }

        public Task<LinkedIntegration> GetLinkedIntegrationByIntegrationIdAsync(int accountId, int integrationId)
        {
            var linkedIntegration = _repository.Execute(new GetLinkedIntegrationById(accountId, integrationId));

            return linkedIntegration;
        }

        public async Task<LinkedIntegration> AddAsync(LinkedIntegration linkedIntegration, bool saveChanges = true)
        {
            return await _repository.AddAsync(linkedIntegration, saveChanges);
        }

        public async Task<LinkedIntegration> UpdateAsync(LinkedIntegration linkedIntegration, bool saveChanges = true)
        {
            return await _repository.UpdateAsync(linkedIntegration, saveChanges);
        }

        public async Task DeleteAsync(int accountId, int integrationId, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetLinkedIntegrationById(accountId, integrationId));

            await _repository.DeleteAsync(entityToUpdate, saveChanges);
        }
    }
}