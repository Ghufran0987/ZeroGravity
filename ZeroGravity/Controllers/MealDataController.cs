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
    public class MealDataController : BaseController
    {
        private readonly IMealDataService _service;

        public MealDataController(IMealDataService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("filtered")]
        public async Task<ActionResult<List<MealDataDto>>> GetByAccountIdWithFilter(FilterMealDataDto dto)
        {
            VerifyAccountId(dto.AccountId);
            var mealDatas = await _service.GetWithFilter(dto);
            List<MealDataDto> mealDataDtos = new List<MealDataDto>();

            foreach (var mealData in mealDatas)
            {
                var mealDataDto = DtoConverter.GetMealDataDto(mealData);
                var mealIngredientsDto = DtoConverter.GetMealIngredientsDto(mealData);
                mealDataDto.Ingredients = mealIngredientsDto;

                mealDataDtos.Add(mealDataDto);
            }

            return Ok(mealDataDtos);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MealDataDto>> GetById(int id, bool getIngredients = false)
        {
            var mealData = await _service.GetByIdAsync(id, getIngredients);

            if (mealData != null)
            {
                var mealDataDto = DtoConverter.GetMealDataDto(mealData);

                var mealIngredientsDto = DtoConverter.GetMealIngredientsDto(mealData);
                mealDataDto.Ingredients = mealIngredientsDto;

                return Ok(mealDataDto);
            }

            return Ok(new MealDataDto());
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<MealDataDto>>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var mealDatas = await _service.GetByAccoundAndDateAsync(id, date);

            List<MealDataDto> mealDataDtos = new List<MealDataDto>();

            foreach (var mealData in mealDatas)
            {
                var mealDataDto = DtoConverter.GetMealDataDto(mealData);

                mealDataDtos.Add(mealDataDto);
            }

            return Ok(mealDataDtos);
        }

        [Authorize]
        [HttpGet("{id:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<ActionResult<List<MealDataDto>>> GetByAccountId(int id, DateTime fromDate, DateTime toDate)
        {
            VerifyAccountId(id);
            var mealDatas = await _service.GetByAccountAndDateRangeAsync(id, fromDate, toDate);
            List<MealDataDto> mealDataDtos = new List<MealDataDto>();
            foreach (var mealData in mealDatas)
            {
                var mealDataDto = DtoConverter.GetMealDataDto(mealData);
                mealDataDtos.Add(mealDataDto);
            }
            return Ok(mealDataDtos);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<MealDataDto>> Create(MealDataDto mealDataDto)
        {
            VerifyAccountId(mealDataDto.AccountId);
            var mealData = DtoConverter.GetMealData(mealDataDto);

            var mealIngredients = DtoConverter.GetMealIngredients(mealDataDto);
            mealData.Ingredients = mealIngredients;

            mealData = await _service.AddAsync(mealData);

            mealDataDto = DtoConverter.GetMealDataDto(mealData);
            mealDataDto.Ingredients = DtoConverter.GetMealIngredientsDto(mealData);

            return Ok(mealDataDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<MealDataDto>> Update(MealDataDto mealDataDto)
        {
            VerifyAccountId(mealDataDto.AccountId);
            var mealData = DtoConverter.GetMealData(mealDataDto);
            var mealIngredients = DtoConverter.GetMealIngredients(mealDataDto);
            mealData.Ingredients = mealIngredients;

            mealData = await _service.UpdateAsync(mealData);

            mealDataDto = DtoConverter.GetMealDataDto(mealData);
            mealDataDto.Ingredients = DtoConverter.GetMealIngredientsDto(mealData);

            return Ok(mealDataDto);
        }
    }
}