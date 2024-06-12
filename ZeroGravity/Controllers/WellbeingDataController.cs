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
    public class WellbeingDataController : BaseController
    {
        private readonly IWellbeingDataService _wellbeingDataService;

        public WellbeingDataController(IWellbeingDataService wellbeingDataService)
        {
            _wellbeingDataService = wellbeingDataService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WellbeingDataDto>> GetById(int id)
        {
            var wellbeingData = await _wellbeingDataService.GetByIdAsync(id);

            if (wellbeingData != null)
            {
                var wellbeingDataDto = DtoConverter.GetWellbeingDataDto(wellbeingData);

                return Ok(wellbeingDataDto);
            }

            return Ok(new WellbeingDataDto());
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<WellbeingDataDto>>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var wellbeingDatas = await _wellbeingDataService.GetAllByAccountIdAndDateAsync(id, date);
            List<WellbeingDataDto> wellbeingDtos = new List<WellbeingDataDto>();
            foreach(var wellbeingData in wellbeingDatas)
            {
                var wellbeingDataDto = DtoConverter.GetWellbeingDataDto(wellbeingData);
                wellbeingDtos.Add(wellbeingDataDto);
            }
            return Ok(wellbeingDtos);
        }

        [Authorize]
        [HttpGet("{id:int}/{dateFrom:DateTime}/{dateTo:DateTime}")]
        public async Task<ActionResult<List<WellbeingDataDto>>> GetByAccountId(int id, DateTime dateFrom, DateTime dateTo)
        {
            VerifyAccountId(id);
            var wellbeingDatas = await _wellbeingDataService.GetAllByAccountIdAndDateRangeAsync(id, dateFrom, dateTo);
            List<WellbeingDataDto> wellbeingDtos = new List<WellbeingDataDto>();
            foreach (var wellbeingData in wellbeingDatas)
            {
                var wellbeingDataDto = DtoConverter.GetWellbeingDataDto(wellbeingData);
                wellbeingDtos.Add(wellbeingDataDto);
            }
            return Ok(wellbeingDtos);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<WellbeingDataDto>> Create(WellbeingDataDto wellbeingDataDto)
        {
            VerifyAccountId(wellbeingDataDto.AccountId);
            var wellbeingData = DtoConverter.GetWellbeingData(wellbeingDataDto);

            wellbeingData = await _wellbeingDataService.AddAsync(wellbeingData);

            wellbeingDataDto = DtoConverter.GetWellbeingDataDto(wellbeingData);

            return Ok(wellbeingDataDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<WellbeingDataDto>> Update(WellbeingDataDto wellbeingDataDto)
        {
            VerifyAccountId(wellbeingDataDto.AccountId);
            var wellbeingData = DtoConverter.GetWellbeingData(wellbeingDataDto);

            wellbeingData = await _wellbeingDataService.UpdateAsync(wellbeingData);

            wellbeingDataDto = DtoConverter.GetWellbeingDataDto(wellbeingData);

            return Ok(wellbeingDataDto);
        }
    }
}