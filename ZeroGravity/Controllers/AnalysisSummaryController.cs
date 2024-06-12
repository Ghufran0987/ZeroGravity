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
    public class AnalysisSummaryController : BaseController
    {
        private readonly IActivityService _activityService;
        private readonly ILiquidIntakeService _liquidIntakeService;
        private readonly IMealDataService _mealDataService;
        private readonly IPersonalGoalsService _personalGoalsService;

        public AnalysisSummaryController(IMealDataService mealDataService, ILiquidIntakeService liquidIntakeService,
            IActivityService activityService, IPersonalGoalsService personalGoalsService)
        {
            _personalGoalsService = personalGoalsService;
            _activityService = activityService;
            _liquidIntakeService = liquidIntakeService;
            _mealDataService = mealDataService;
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<AnalysisSummaryDto>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var liquidIntakes = await _liquidIntakeService.GetByAccoundAndDateAsync(id, date);
            var mealDatas = await _mealDataService.GetByAccoundAndDateAsync(id, date);
            var activityDatas = await _activityService.GetAllByAccountIdAndDateAsync(id, date);
            var personalGoal = await _personalGoalsService.GetByAccounIdAsync(id);

            var analysisSummaryDto = new AnalysisSummaryDto
            {
                ActivityDataDtos = activityDatas.Select(DtoConverter.GetActivityDataDto).ToList(),
                LiquidIntakeDtos = liquidIntakes.Select(DtoConverter.GetLiquidIntakeDto).ToList(),
                MealDataDtos = mealDatas.Select(DtoConverter.GetMealDataDto).ToList(),
                PersonalGoalDto = DtoConverter.GetPersonalGoalDto(personalGoal)
            };


            return Ok(analysisSummaryDto);
        }
    }
}