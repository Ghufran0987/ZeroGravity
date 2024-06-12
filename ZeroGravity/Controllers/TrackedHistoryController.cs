using System;
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
    public class TrackedHistoryController : BaseController
    {
        private readonly ITrackedHistorieService _trackedHistorieService;

        public TrackedHistoryController(ITrackedHistorieService trackedHistorieService)
        {
            _trackedHistorieService = trackedHistorieService;
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<TrackedHistoryDto>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var trackedHistoryDtos = await _trackedHistorieService.GetAllByAccountIdAndDateAsync(id, date);

            return Ok(trackedHistoryDtos);
        }
    }
}