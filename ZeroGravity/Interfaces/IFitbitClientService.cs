using System;
using System.Threading.Tasks;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IFitbitClientService
    {
        Task<FitbitAccountDto> GetFitbitAuthenticationInfo(int accountId);
        Task<bool> GetFitbitUserTokenByCode(FitbitCallbackDto fitbitCallbackParams);
        Task<FitbitActivityDataDto> GetFitbitActivitiesAsync(int accountId, int integrationId, DateTime targetDateTime);
        Task<bool> RefreshFitbitToken(int accountId, int integrationId);
    }
}