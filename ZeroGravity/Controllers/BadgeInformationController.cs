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
    public class BadgeInformationController : BaseController
    {
        private readonly IBadgeinformationService _badgeinformationService;

        public BadgeInformationController(IBadgeinformationService badgeinformationService)
        {
            _badgeinformationService = badgeinformationService;
        }

        [Authorize]
        [HttpGet("activity/{id:int}/{date:DateTime}")]
        public async Task<ActionResult<ActivityBadgeInformationDto>> GetActivityBadgeInformationByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);

            var activityBadgeInformationDto = await _badgeinformationService.GetActivityBadgeInformationAsync(id, date);

            return Ok(activityBadgeInformationDto);
        }

        [Authorize]
        [HttpGet("liquid/{id:int}/{date:DateTime}")]
        public async Task<ActionResult<LiquidIntakeBadgeInformationDto>> GetLiquidIntakeBadgeInformationByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var liquidIntakeBadgeInformationDto = await _badgeinformationService.GetLiquidIntakeBadgeInformationAsync(id, date);

            return Ok(liquidIntakeBadgeInformationDto);
        }

        [Authorize]
        [HttpGet("meals/{id:int}/{date:DateTime}")]
        public async Task<ActionResult<MealsBadgeInformationDto>> GetMealsBadgeInformationByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var mealsBadgeInformationDto = await _badgeinformationService.GetMealsBadgeInformationAsync(id, date);

            return Ok(mealsBadgeInformationDto);
        }

        [Authorize]
        [HttpGet("myday/{id:int}/{date:DateTime}")]
        public async Task<ActionResult<MyDayBadgeInformationDto>> GetMyDayBadgeInformationByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var myDayBadgeInformationDto = await _badgeinformationService.GetMyDayBadgeInformationAsync(id, date);

            return Ok(myDayBadgeInformationDto);
        }
    }
}