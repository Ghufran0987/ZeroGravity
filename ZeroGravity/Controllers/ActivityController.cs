using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : BaseController
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityDataDto>> GetById(int id)
        {
            var activityData = await _activityService.GetByIdAsync(id);

            if (activityData != null)
            {
                var activityDataDto = DtoConverter.GetActivityDataDto(activityData);

                return Ok(activityDataDto);
            }

            return Ok(new ActivityDataDto());
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<ActivityDataDto>>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var activityDatas = await _activityService.GetAllByAccountIdAndDateAsync(id, date);

            List<ActivityDataDto> activityDataDtos = new List<ActivityDataDto>();
            foreach (var activityData in activityDatas)
            {
                var activityDataDto = DtoConverter.GetActivityDataDto(activityData);
                activityDataDtos.Add(activityDataDto);
            }
            return Ok(activityDataDtos);
        }

        [Authorize]
        [HttpGet("{id:int}/{dateFrom:DateTime}/{dateTo:DateTime}")]
        public async Task<ActionResult<List<ActivityDataDto>>> GetByAccountId(int id, DateTime dateFrom, DateTime dateTo)
        {
            VerifyAccountId(id);
            var activityDatas = await _activityService.GetAllByAccountIdAndDateRangeAsync(id, dateFrom, dateTo);

            List<ActivityDataDto> activityDataDtos = new List<ActivityDataDto>();
            foreach (var activityData in activityDatas)
            {
                var activityDataDto = DtoConverter.GetActivityDataDto(activityData);
                activityDataDtos.Add(activityDataDto);
            }
            return Ok(activityDataDtos);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<ActivityDataDto>> Create(ActivityDataDto activityDataDto)
        {
            VerifyAccountId(activityDataDto?.AccountId);
            var activityData = DtoConverter.GetActivityData(activityDataDto);

            activityData = await _activityService.AddAsync(activityData);
            if (activityData == null) return null;
            if (activityData.Id > 0)
            {
                // Find next coming monday and schedule for new goal calulation
                // Testing only _activityService.ComputeNewActivityGoalAsync(activityData.AccountId);

                var jobId = $"ActivityGoal-{activityData.AccountId}";
                RecurringJob.AddOrUpdate(jobId, () => _activityService.ComputeNewActivityGoalAsync(activityData.AccountId), Cron.Weekly);

            }
            activityDataDto = DtoConverter.GetActivityDataDto(activityData);
            return Ok(activityDataDto);
        }

        [Authorize]
        [HttpPost("sync")]
        public async Task<ActionResult<ActivityDataDto>> Sync(ActivityDataDto activityDataDto)
        {
            VerifyAccountId(activityDataDto?.AccountId);
            var activityData = DtoConverter.GetActivityData(activityDataDto);

            activityData = await _activityService.ImportActivityAsync(activityData);

            activityDataDto = DtoConverter.GetActivityDataDto(activityData);

            return Ok(activityDataDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<ActivityDataDto>> Update(ActivityDataDto activityDataDto)
        {
            VerifyAccountId(activityDataDto?.AccountId);
            var activityData = DtoConverter.GetActivityData(activityDataDto);

            activityData = await _activityService.UpdateAsync(activityData);

            activityDataDto = DtoConverter.GetActivityDataDto(activityData);

            return Ok(activityDataDto);
        }
    }
}