using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMyWeightPageVmProvider : IPageVmProvider
    {

        Task<ApiCallResult<WeightItemProxy>> AddWeightDataAsync(WeightItemProxy weightDataDto, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightItemDetailsProxy>> AddWeightDetailsDataAsync(WeightItemDetailsProxy proxy, CancellationToken cancellationToken);


        Task<ApiCallResult<List<WeightItemProxy>>> GetAll(int accountId, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightItemDetailsProxy>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightItemDetailsProxy>> GetCurrentWeightTrackerAsync(CancellationToken cancellationToken);

        Task<ApiCallResult<WeightItemDetailsProxy>> UpdateAsync(WeightItemDetailsProxy weightDetailsDto, CancellationToken cancellationToken);

    }
}
