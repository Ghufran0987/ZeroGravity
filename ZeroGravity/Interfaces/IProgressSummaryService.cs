using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IProgressSummaryService
    {
        Task<List<ProgressSummaryDto>> GetByIdForDayAsync(int id, DateTime fromDate);

        Task<List<ProgressSummaryDto>> GetByIdForPeriodAsync(int id, DateTime fromDate, DateTime toDate);
    }
}