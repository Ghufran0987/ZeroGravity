using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBeatEatingSessionDataController : BaseController
    {
        private readonly ISugarBeatEatingSessionDataService _sugarBeatSessionDataService;

        public SugarBeatEatingSessionDataController(ISugarBeatEatingSessionDataService sugarBeatSessionDataService)
        {
            _sugarBeatSessionDataService = sugarBeatSessionDataService;
        }

        [HttpPost("create")]
        public async Task<SugarBeatEatingSessionDto> AddAsync(SugarBeatEatingSessionDto sessionDto, bool saveChanges = true)
        {
            var sessionData = DtoConverter.GetSugarBeatEatingSessionData(sessionDto);
            var result = await _sugarBeatSessionDataService.AddAsync(sessionData, saveChanges);
            if (result == null) return null;
            if (result.Id > 0)
            {
                // MetaScore now calculated in the app
                //var jobId = BackgroundJob.Schedule(() => _sugarBeatSessionDataService.ComputeMetabolicScoreAsync(result.Id), new TimeSpan(1, 30, 0));
            }
            var sugarBeatSessionDto = DtoConverter.GetSugarBeatEatingSessionDto(result);
            return sugarBeatSessionDto;
        }

        [HttpPut("update")]
        public async Task<SugarBeatEatingSessionDto> UpdateAsync(SugarBeatEatingSessionDto sessionDto, bool saveChanges = true)
        {
            SugarBeatEatingSession updatedEatingSession = await _sugarBeatSessionDataService.UpdateAsync(
                DtoConverter.GetSugarBeatEatingSessionData(sessionDto),
                saveChanges
            );

            SugarBeatEatingSessionDto updatedEatingSessionDto = null;
            if(updatedEatingSession != null)
            {
                updatedEatingSessionDto = DtoConverter.GetSugarBeatEatingSessionDto(updatedEatingSession);
            }
            return updatedEatingSessionDto;
        }

        [HttpGet("{id:int}")]
        public async Task<SugarBeatEatingSessionDto> GetByIdAsync(int id)
        {
            var result = await _sugarBeatSessionDataService.GetByIdAsync(id);
            if (result == null) return null;
            var sugarBeatSessionDto = DtoConverter.GetSugarBeatEatingSessionDto(result);
            return sugarBeatSessionDto;
        }

        [HttpGet("{accountId:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<List<SugarBeatEatingSessionDto>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var retResult = new List<SugarBeatEatingSessionDto>();
            var result = await _sugarBeatSessionDataService.GetSessionForPeriodAsync(accountId, fromDate, toDate);
            if (result == null) return null;
            foreach (var data in result)
            {
                var sugarBeatSessionDto = DtoConverter.GetSugarBeatEatingSessionDto(data);
                retResult.Add(sugarBeatSessionDto);
            }
            return retResult;
        }
    }
}