using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgressSummaryController : BaseController
    {
        private readonly IProgressSummaryService _progressSummaryService;

        public ProgressSummaryController(IProgressSummaryService progressSummaryService)
        {
            _progressSummaryService = progressSummaryService;
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<ProgressSummaryDto>>> GetByAccountIdForDay(int id, DateTime date)
        {
            VerifyAccountId(id);
            var result = await _progressSummaryService.GetByIdForDayAsync(id, date);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<ActionResult<List<ProgressSummaryDto>>> GetByAccountIdForPeriod(int id, DateTime fromDate, DateTime toDate)
        {
            VerifyAccountId(id);
            var result = await _progressSummaryService.GetByIdForPeriodAsync(id, fromDate, toDate);
            return Ok(result);
        }
    }
}