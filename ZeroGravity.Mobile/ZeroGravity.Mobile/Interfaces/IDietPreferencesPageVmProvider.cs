using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IDietPreferencesPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<DietPreferencesProxy>> GetDietPreferencesAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<DietPreferencesProxy>> CreateDietPreferencesAsnyc(DietPreferencesProxy dietPreferencesProxy, CancellationToken cancellationToken);

        Task<ApiCallResult<DietPreferencesProxy>> UpdateDietPreferencesAsnyc(DietPreferencesProxy dietPreferencesProxy, CancellationToken cancellationToken);
    }
}