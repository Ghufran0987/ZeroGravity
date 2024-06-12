using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ILiquidIntakeDataService
    {
        Task<ApiCallResult<LiquidIntakeDto>> GetLiquidIntakeDataAsync(int liquidIntakeId, CancellationToken cancellationToken);
        Task<ApiCallResult<List<LiquidIntakeDto>>> GetLiquidIntakeDataByDateAsync(DateTime targetDateTime, LiquidType type, CancellationToken cancellationToken);

        Task<ApiCallResult<LiquidIntakeDto>> CreateLiquidIntakeDataAsync(LiquidIntakeDto liquidIntakeDto, CancellationToken cancellationToken);

        Task<ApiCallResult<LiquidIntakeDto>> UpdateLiquidIntakeDataAsync(LiquidIntakeDto liquidIntakeDto, CancellationToken cancellationToken);
    }
}