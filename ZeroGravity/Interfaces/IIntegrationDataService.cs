using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IIntegrationDataService
    {
        Task<List<IntegrationData>> GetAvailableIntegrationsAsync();
        Task<IntegrationData> GetIntegrationByNameAsync(string integrationName);
        Task<List<LinkedIntegration>> GetLinkedIntegrationsByAccountIdAsync(int accountId);
        Task<LinkedIntegration> GetLinkedIntegrationByIntegrationIdAsync(int accountId, int integrationId);
        Task<LinkedIntegration> AddAsync(LinkedIntegration linkedIntegration, bool saveChanges = true);
        Task DeleteAsync(int accountId, int integrationId, bool saveChanges = true);
        Task<LinkedIntegration> UpdateAsync(LinkedIntegration linkedIntegration, bool saveChanges = true);
    }
}