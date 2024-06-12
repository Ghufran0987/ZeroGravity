using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Interfaces
{
public    interface ISugarBeatSettingsSevice
    {

        Task<ApiCallResult<SugarBeatSettingDto>> AddAsync(SugarBeatSettingDto settingDto, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatSettingDto>> UpdateAsync(SugarBeatSettingDto settingDto, CancellationToken cancellationToken, bool saveChanges = true);

        Task<ApiCallResult<SugarBeatSettingDto>> GetByAccountIdAsync( CancellationToken cancellationToken);

    }
}
