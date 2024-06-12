using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class ProgressSummaryService : IProgressSummaryService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public ProgressSummaryService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<List<ProgressSummaryDto>> GetByIdForDayAsync(int id, DateTime fromDate)
        {
            var result = _repository.Execute(new GetProgressByAccountIdForDay(id, fromDate));
            return result;
        }

        public Task<List<ProgressSummaryDto>> GetByIdForPeriodAsync(int id, DateTime fromDate, DateTime toDate)
        {
            var result = _repository.Execute(new GetProgressByAccountIdForPeriod(id, fromDate, toDate));
            return result;
        }
    }
}