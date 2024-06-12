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
    public class LiquidNutritionController : BaseController
    {
        private readonly ILiquidNutritionService _service;

        public LiquidNutritionController(ILiquidNutritionService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{liquidIntakeId:int}")]
        public async Task<ActionResult<MealDataDto>> GetById(int liquidIntakeId, bool includeLiquidComponentNutrition = false)
        {
            var liquidNutrition = await _service.GetByLiquidIntakeIdAsync(liquidIntakeId, includeLiquidComponentNutrition);

            if (liquidNutrition != null)
            {
                var liquidNutritionDto = DtoConverter.GetLiquidNutritionDto(liquidNutrition);

                return Ok(liquidNutritionDto);
            }

            return NotFound(null);
        }
    }
}