using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWeightDataService
    {

        Task<ApiCallResult<WeightDto>> AddWeightDataAsync(WeightDto weightDataDto, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightDetailsDto>> AddWeightDetailsDataAsync(WeightDetailsDto proxy, CancellationToken cancellationToken);

        Task<ApiCallResult<List<WeightDto>>> GetAll(int accountId, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightDetailsDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ApiCallResult<WeightDetailsDto>> GetCurrentWeightTrackerAsync( CancellationToken cancellationToken);

        Task<ApiCallResult<WeightDetailsDto>> UpdateAsync(WeightDetailsDto weightDetailsDto, CancellationToken cancellationToken);

    }
}
