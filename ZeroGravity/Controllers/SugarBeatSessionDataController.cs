using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBeatSessionDataController : BaseController
    {
        private readonly ISugarBeatSessionDataService _sugarBeatSessionDataService;

        public SugarBeatSessionDataController(ISugarBeatSessionDataService sugarBeatSessionDataService)
        {
            _sugarBeatSessionDataService = sugarBeatSessionDataService;
        }

        // [HttpPost("create")]
        // public async Task<SugarBeatSessionDto> AddAsync(SugarBeatSessionDto sessionDto, bool saveChanges = true)
        // {
        //     var sessionData= DtoConverter.GetSugarBeatSessionData(sessionDto);
        //     var result = await _sugarBeatSessionDataService.AddAsync(sessionData, saveChanges);
        //     var sugarBeatSessionDto = DtoConverter.GetSugarBeatSessionDto(result);
        //     return sugarBeatSessionDto;
        // }

        [HttpPut("update")]
        public async Task<SugarBeatSessionDto> UpdateAsync(SugarBeatSessionDto sessionDto, bool saveChanges = true)
        {
            var session = await _sugarBeatSessionDataService.GetByIdAsync(sessionDto.Id, false);
            if (session != null)
            {
                // Only update End Time From Outside
                session.EndTime = sessionDto.EndTime;
                var result = await _sugarBeatSessionDataService.UpdateAsync(session, saveChanges);

                var sugarBeatSessionDto = DtoConverter.GetSugarBeatSessionDto(result);
                return sugarBeatSessionDto;
            }
            return null;
        }

        [HttpGet("{accountId:int}/{targetDate:DateTime}/active")]
        public async Task<SugarBeatSessionDto> GetActiveSessionAsync(int accountId, DateTime targetDate, bool includeGlucoseData = false)
        {
            var result = await _sugarBeatSessionDataService.GetActiveSessionAsync(accountId, targetDate, includeGlucoseData);
            if (result == null) return null;
            var sugarBeatSessionDto = DtoConverter.GetSugarBeatSessionDto(result);
            return sugarBeatSessionDto;
        }

        [HttpGet("{id:int}")]
        public async Task<SugarBeatSessionDto> GetByIdAsync(int id, bool includeGlucoseData = false)
        {
            var result = await _sugarBeatSessionDataService.GetByIdAsync(id, includeGlucoseData);
            if (result == null) return null;
            var sugarBeatSessionDto = DtoConverter.GetSugarBeatSessionDto(result);
            return sugarBeatSessionDto;
        }

        [HttpGet("IsSessionWarmedUp/{sessionId:int}")]
        public async Task<bool> GetIsSessionWarmedUpAsync(int sessionId)
        {
            var result = await _sugarBeatSessionDataService.GetIsSessionWarmedUpAsync(sessionId);
            return result;
        }

        [HttpGet("{accountId:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<List<SugarBeatSessionDto>> GetSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool includeGlucoseData = false)
        {
            var retResult = new List<SugarBeatSessionDto>();
            var result = await _sugarBeatSessionDataService.GetSessionForPeriodAsync(accountId, fromDate, toDate, includeGlucoseData);
            if (result == null) return null;
            foreach (var data in result)
            {
                var sugarBeatSessionDto = DtoConverter.GetSugarBeatSessionDto(data);
                retResult.Add(sugarBeatSessionDto);
            }
            return retResult;
        }

        [HttpGet("{accountId:int}/metabolicScore/latest")]
        public async Task<object> GetLatestMetabolicScoreAsync(int accountId)
        {
            int score = await _sugarBeatSessionDataService.GetLatestMetabolicScoreAsync(accountId);
            return new { metabolicScore = score };
        }
    }
}