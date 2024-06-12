using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealNutritionController : BaseController
    {
        private readonly IMealNutritionService _service;

        public MealNutritionController(IMealNutritionService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{mealDataId:int}")]
        public async Task<ActionResult<MealDataDto>> GetById(int mealDataId, bool includeMealComponentNutritionAndFoodSwaps = false)
        {
            var mealNutrition = await _service.GetByMealDataIdAsync(mealDataId, includeMealComponentNutritionAndFoodSwaps);

            if (mealNutrition != null)
            {
                var mealNutritionDto = DtoConverter.GetMealNutritionDto(mealNutrition);

                return Ok(mealNutritionDto);
            }

            return NotFound(null);
        }
    }
}