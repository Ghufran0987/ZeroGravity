using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFastingSettingsPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<FastingSettingProxy>> GetFastingSettingAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<FastingSettingProxy>> CreateFastingDataAsnyc(FastingSettingProxy fastingSettingProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingSettingProxy>> UpdateFastingSettingAsync(FastingSettingProxy fastingSettingProxy, CancellationToken cancellationToken);
    }
}