using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBeatAlertDataController : BaseController
    {
        private readonly ISugarBeatAlertDataService _sugarBeatAlertDataService;

        public SugarBeatAlertDataController(ISugarBeatAlertDataService sugarBeatAlertDataService)
        {
            _sugarBeatAlertDataService = sugarBeatAlertDataService;
        }

        [HttpPost("create")]
        public async Task<SugarBeatAlertDto> AddAsync(SugarBeatAlertDto alertDto, bool saveChanges = true)
        {
            var alertData = DtoConverter.GetSugarBeatAlertData(alertDto);
            // Create Session if ALERT_PATCH_CONNECTED from db trigger and
            // Update end time of session if already not updated using db trigger

            SugarBeatAlertData alert = await _sugarBeatAlertDataService.AddAsync(alertData, saveChanges);
            SugarBeatAlertDto sugarBeatAlertDto = DtoConverter.GetSugarBeatAlertDto(alert);
            return sugarBeatAlertDto;
        }

        // Should not expose alert update as it's only to be inserted
        //[HttpPut("update")]
        //public async Task<SugarBeatAlertDto> UpdateAsync(SugarBeatAlertData alertData, bool saveChanges = true)
        //{
        //    var result = await _sugarBeatAlertDataService.UpdateAsync(alertData, saveChanges);
        //    var sugarBeatAlertDto = DtoConverter.GetSugarBeatAlertDto(result);
        //    return sugarBeatAlertDto;
        //}

        [HttpGet("{id:int}")]
        public async Task<SugarBeatAlertDto> GetByIdAsync(int id)
        {
            var result = await _sugarBeatAlertDataService.GetByIdAsync(id);
            if (result == null) return null;
            var sugarBeatAlertDto = DtoConverter.GetSugarBeatAlertDto(result);
            return sugarBeatAlertDto;
        }

        [HttpGet("{accountId:int}/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<List<SugarBeatAlertDto>> GetAlertForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, AlertCode? alertCode = null, CRCCodes? criticalCode = null)
        {
            var retResult = new List<SugarBeatAlertDto>();
            var result = await _sugarBeatAlertDataService.GetAlertForPeriodAsync(accountId, fromDate, toDate, alertCode, criticalCode);
            if (result == null) return null;
            foreach (var data in result)
            {
                var sugarBeatAlertDto = DtoConverter.GetSugarBeatAlertDto(data);
                retResult.Add(sugarBeatAlertDto);
            }
            return retResult;
        }
    }
}