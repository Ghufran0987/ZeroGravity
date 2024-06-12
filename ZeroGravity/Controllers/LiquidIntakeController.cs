using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LiquidIntakeController: BaseController
    {
        private readonly ILiquidIntakeService _liquidIntakeService;

        public LiquidIntakeController(ILiquidIntakeService liquidIntakeService)
        {
            _liquidIntakeService = liquidIntakeService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FastingDataDto>> GetById(int id)
        {
            var liquidIntake = await _liquidIntakeService.GetByIdAsync(id);

            if (liquidIntake != null)
            {
                var liquidIntakeDataDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);

                return Ok(liquidIntakeDataDto);
            }

            return Ok(new LiquidIntakeDto());
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}/{type:int}")]
        public async Task<ActionResult<List<LiquidIntakeDto>>> GetByAccountIdWithType(int id, DateTime date, LiquidType type)
        {
            VerifyAccountId(id);
            var liquidIntakes = await _liquidIntakeService.GetByAccountAndDateAndTypeAsync(id, date, type);

            if (liquidIntakes.Any())
            {
                var liquidIntakeDtoList = new List<LiquidIntakeDto>();

                foreach (var liquidIntake in liquidIntakes)
                {
                    var liquidIntakeDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);

                    liquidIntakeDtoList.Add(liquidIntakeDto);
                }

                return Ok(liquidIntakeDtoList);
            }

            return Ok(new List<LiquidIntakeDto>());
        }


        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<LiquidIntakeDto>>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var liquidIntakes = await _liquidIntakeService.GetByAccoundAndDateAsync(id, date);

            if (liquidIntakes.Any())
            {
                var liquidIntakeDtoList = new List<LiquidIntakeDto>();

                foreach (var liquidIntake in liquidIntakes)
                {
                    var liquidIntakeDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);

                    liquidIntakeDtoList.Add(liquidIntakeDto);
                }

                return Ok(liquidIntakeDtoList);
            }

            return Ok(new List<LiquidIntakeDto>());
        }

        [Authorize]
        [HttpGet("{id:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<ActionResult<List<LiquidIntakeDto>>> GetByAccountId(int id, DateTime fromDate, DateTime toDate)
        {
            VerifyAccountId(id);
            var liquidIntakes = await _liquidIntakeService.GetByAccountAndDateRangeAsync(id, fromDate, toDate);
            var liquidIntakeDtoList = new List<LiquidIntakeDto>();
            if (liquidIntakes.Any())
            {
                foreach (var liquidIntake in liquidIntakes)
                {
                    var liquidIntakeDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);
                    liquidIntakeDtoList.Add(liquidIntakeDto);
                }
            }
            return Ok(liquidIntakeDtoList);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<LiquidIntakeDto>> Create(LiquidIntakeDto liquidIntakeDto)
        {
            VerifyAccountId(liquidIntakeDto.AccountId);
            var liquidIntake = DtoConverter.GetLiquidIntake(liquidIntakeDto);

            liquidIntake = await _liquidIntakeService.AddAsync(liquidIntake);

            liquidIntakeDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);

            return Ok(liquidIntakeDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<LiquidIntakeDto>> Update(LiquidIntakeDto liquidIntakeDto)
        {
            VerifyAccountId(liquidIntakeDto.AccountId);
            var liquidIntake = DtoConverter.GetLiquidIntake(liquidIntakeDto);

            liquidIntake = await _liquidIntakeService.UpdateAsync(liquidIntake);

            liquidIntakeDto = DtoConverter.GetLiquidIntakeDto(liquidIntake);

            return Ok(liquidIntakeDto);
        }
    }
}
