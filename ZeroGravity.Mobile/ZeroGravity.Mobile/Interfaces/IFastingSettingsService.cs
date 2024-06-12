using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFastingSettingsService
    {
        Task<ApiCallResult<FastingSettingDto>> GetFastingSettingByIdAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<FastingSettingDto>> CreateFastingSettingAsync(FastingSettingDto fastingSettingDto, CancellationToken cancellationToken);
        Task<ApiCallResult<FastingSettingDto>> UpdateFastingSettingAsync(FastingSettingDto fastingSettingDto, CancellationToken cancellationToken);
    }
}
