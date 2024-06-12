using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlucoseDataController : BaseController
    {
        private readonly IGlucoseDataService _glucoseDataService;

        public GlucoseDataController(IGlucoseDataService glucoseDataService)
        {
            _glucoseDataService = glucoseDataService;
        }


        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<GlucoseDataDto>> Create(GlucoseDataDto glucoseDataDto)
        {
            VerifyAccountId(glucoseDataDto?.AccountId);

            var glucoseData = DtoConverter.GetGlucoseData(glucoseDataDto);

            if (glucoseDataDto.SugarBeatData != null && glucoseDataDto.SugarBeatData.Any())
            {
                var sugarBeatData = DtoConverter.GetSugarBeatData(glucoseDataDto);
                glucoseData.SugarBeatData = sugarBeatData;
            }

            glucoseData = await _glucoseDataService.AddAsync(glucoseData);

            glucoseDataDto = DtoConverter.GetGlucoseDataDto(glucoseData);

            return Ok(glucoseDataDto);
        }

        //[Authorize]
        //[HttpPut("update")]
        //public async Task<ActionResult<GlucoseDataDto>> Update(GlucoseDataDto glucoseDataDto)
        //{
        //    var glucoseData = DtoConverter.GetGlucoseData(glucoseDataDto);

        //    var sugarBeatData = DtoConverter.GetSugarBeatData(glucoseDataDto);
        //    glucoseData.SugarBeatData = sugarBeatData;

        //    glucoseData = await _glucoseDataService.UpdateAsync(glucoseData);

        //    glucoseDataDto = DtoConverter.GetGlucoseDataDto(glucoseData);

        //    return Ok(glucoseDataDto);
        //}


        //[Authorize]
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<GlucoseDataDto>> GetById(int id, bool getSugarBeatData)
        //{
        //    var glucoseData = await _glucoseDataService.GetByIdAsync(id, getSugarBeatData);

        //    if (glucoseData != null)
        //    {
        //        var glucoseDataDto = DtoConverter.GetGlucoseDataDto(glucoseData);

        //        var sugarBeatDataDto = DtoConverter.GetSugarBeatDataDto(glucoseData);
        //        glucoseDataDto.SugarBeatData = sugarBeatDataDto;

        //        return Ok(glucoseDataDto);
        //    }

        //    return Ok(new GlucoseDataDto());
          
        //}

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<GlucoseDataDto>>> GetByAccountId(int id, DateTime date, string type, bool getSugarBeatData)
        {
            VerifyAccountId(id);

            var glucoseDatas = new List<GlucoseData>();

            switch (type)
            {
                case GlucoseConstants.GlucoseTypeAll:
                    glucoseDatas = await _glucoseDataService.GetByAccountAndDateForAllAsync(id, date);
                    break;

                case GlucoseConstants.GlucoseTypeManual:
                    glucoseDatas = await _glucoseDataService.GetByAccountAndDateForManualAsync(id, date);
                    break;

                case GlucoseConstants.GlucoseTypeSugarBeat:
                    glucoseDatas = await _glucoseDataService.GetByAccountAndDateForSugarBeatAsync(id, date);
                    break;
            }

            if (glucoseDatas != null)
            {
                List<GlucoseDataDto> glucoseDataDtos = new List<GlucoseDataDto>();

                if (getSugarBeatData)
                {
                    foreach (var glucoseData in glucoseDatas)
                    {
                        var glucoseDataDto = DtoConverter.GetGlucoseDataDto(glucoseData);
                        var sugarBeatDataDto = DtoConverter.GetSugarBeatDataDto(glucoseData);
                        glucoseDataDto.SugarBeatData = sugarBeatDataDto;

                        glucoseDataDtos.Add(glucoseDataDto);
                    }
                }
                else
                {
                    foreach (var glucoseData in glucoseDatas)
                    {
                        var glucoseDataDto = DtoConverter.GetGlucoseDataDto(glucoseData);

                        glucoseDataDtos.Add(glucoseDataDto);
                    }
                }

                return Ok(glucoseDataDtos);
            }

            return Ok(new List<GlucoseDataDto>());
        }
    }
}
