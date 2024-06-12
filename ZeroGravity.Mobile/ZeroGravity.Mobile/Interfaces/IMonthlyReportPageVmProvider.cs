using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMonthlyReportPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<ProgressProxy>> GetProgressDataByDateAsync(DateTime fromDate, DateTime toDate,bool isMetascoreDeviceActive, CancellationToken cancellationToken);

        Task<ApiCallResult<AccountResponseDto>> GetAccountDetailsAsync(CancellationToken cancellationToken);
    }
}