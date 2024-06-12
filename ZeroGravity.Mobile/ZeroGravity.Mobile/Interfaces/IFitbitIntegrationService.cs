using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFitbitIntegrationService
    {
        Task<ApiCallResult<FitbitAccountDto>> GetFitbitAccountAsync(CancellationToken cancellationToken);

        Task<ApiCallResult<FitbitActivityDataDto>> GetFitbitActivitiesAsync(int integrationId, 
            DateTime targetDateTime, CancellationToken cancellationToken);
    }
}
