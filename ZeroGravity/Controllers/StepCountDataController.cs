using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StepCountDataController : BaseController
    {
        private readonly IStepCountDataService _stepCountDataService;

        public StepCountDataController(IStepCountDataService stepCountDataService)
        {
            _stepCountDataService = stepCountDataService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StepCountDataDto>> GetById(int id)
        {
            var stepCountData = await _stepCountDataService.GetByIdAsync(id);

            if (stepCountData != null)
            {
                var stepCountDataDto = DtoConverter.GetStepCountDataDto(stepCountData);

                return Ok(stepCountDataDto);
            }

            return Ok(new StepCountDataDto());
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<StepCountDataDto>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var stepCountData = await _stepCountDataService.GetByAccountIdAndDateAsync(id, date);

            if (stepCountData != null)
            {
                var stepCountDataDto = DtoConverter.GetStepCountDataDto(stepCountData);

                return Ok(stepCountDataDto);
            }

            return Ok(new StepCountDataDto
            {
                AccountId = id,
                TargetDate = date
            });
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<StepCountDataDto>> Create(StepCountDataDto stepCountDataDto)
        {
            VerifyAccountId(stepCountDataDto.AccountId);
            var stepCountData = DtoConverter.GetStepCountData(stepCountDataDto);

            stepCountData = await _stepCountDataService.AddAsync(stepCountData);

            stepCountDataDto = DtoConverter.GetStepCountDataDto(stepCountData);

            return Ok(stepCountDataDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<StepCountDataDto>> Update(StepCountDataDto stepCountDataDto)
        {
            VerifyAccountId(stepCountDataDto.AccountId);
            var stepCountData = DtoConverter.GetStepCountData(stepCountDataDto);

            stepCountData = await _stepCountDataService.UpdateAsync(stepCountData);

            stepCountDataDto = DtoConverter.GetStepCountDataDto(stepCountData);

            return Ok(stepCountDataDto);
        }
    }
}