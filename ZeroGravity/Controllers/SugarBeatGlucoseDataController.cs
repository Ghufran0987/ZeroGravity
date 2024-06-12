using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBeatGlucoseDataController : BaseController
    {
        private readonly ISugarBeatGlucoseDataService _sugarBeatGlucoseDataService;

        public SugarBeatGlucoseDataController(ISugarBeatGlucoseDataService sugarBeatGlucoseDataService)
        {
            _sugarBeatGlucoseDataService = sugarBeatGlucoseDataService;
        }

        [HttpPost("create")]
        public async Task<SugarBeatGlucoseDto> AddAsync(SugarBeatGlucoseDto glucoseDto, bool saveChanges = true)
        {
            var glucoseData = DtoConverter.GetSugarBeatGlucoseData(glucoseDto);
            SugarBeatGlucoseData glucoseMeasurement = await _sugarBeatGlucoseDataService.AddAsync(glucoseData, saveChanges);
            SugarBeatGlucoseDto sugarBeatGlucoseDto = DtoConverter.GetSugarBeatGlucoseDto(glucoseMeasurement);
            return sugarBeatGlucoseDto;
        }

        // Should be done internally through Algorithm
        // public async Task<SugarBeatGlucoseDto> UpdateAsync(SugarBeatGlucoseDto glucoseDto, bool saveChanges = true)
        // {
        //     var glucoseData = DtoConverter.GetSugarBeatGlucoseData(glucoseDto);
        //     var result = await _sugarBeatGlucoseDataService.UpdateAsync(glucoseData, saveChanges);

        //     var sugarBeatGlucoseDto = DtoConverter.GetSugarBeatGlucoseDto(result);

        //     return sugarBeatGlucoseDto;
        // }

        [HttpGet("{accountId:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<List<SugarBeatGlucoseDto>> GetGlucoseForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool isGlucoseNull = false)
        {
            var retResult = new List<SugarBeatGlucoseDto>();
            var result = await _sugarBeatGlucoseDataService.GetGlucoseForPeriodAsync(accountId, fromDate, toDate, isGlucoseNull);
            if (result == null) return null;
            foreach (var data in result)
            {
                var sugarBeatGlucoseDto = DtoConverter.GetSugarBeatGlucoseDto(data);
                retResult.Add(sugarBeatGlucoseDto);
            }
            return retResult;
        }

        [HttpGet("{accountId:int}/{sessionId:int}")]
        public async Task<List<SugarBeatGlucoseDto>> GetAllGlucoseForSessionId(int accountId, int sessionId, bool isGlucoseNull = false)
        {
            var retResult = new List<SugarBeatGlucoseDto>();
            var result = await _sugarBeatGlucoseDataService.GetAllGlucoseForSessionId(accountId, sessionId, isGlucoseNull);
            if (result == null) return null;
            foreach (var data in result)
            {
                var sugarBeatGlucoseDto = DtoConverter.GetSugarBeatGlucoseDto(data);
                retResult.Add(sugarBeatGlucoseDto);
            }
            return retResult;
        }
    }
}