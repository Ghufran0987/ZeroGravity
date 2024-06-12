using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IIntegrationDataService
    {
        Task<ApiCallResult<List<IntegrationDataDto>>> GetIntegrationDataListAsync(
            CancellationToken cancellationToken);

        Task<ApiCallResult<List<IntegrationDataDto>>> GetLinkedIntegrationListAsync(
            CancellationToken cancellationToken);

        Task<ApiCallResult<bool>> DeleteLinkedIntegrationAsync(int integrationId,
            CancellationToken cancellationToken);

        //Task<ApiCallResult<LinkedIntegrationDto>> GetLinkedIntegrationAsync(int integrationId, CancellationToken cancellationToken);
    }
}