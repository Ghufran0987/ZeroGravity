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
    public class MeditationDataController : BaseController
    {
        private readonly IMeditationDataService _meditationDataService;

        public MeditationDataController(IMeditationDataService meditationDataService)
        {
            _meditationDataService = meditationDataService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<MeditationDataDto>> Create(MeditationDataDto meditationDataDto)
        {
            VerifyAccountId(meditationDataDto.AccountId);
            var meditationData = DtoConverter.GetMeditationData(meditationDataDto);

            meditationData = await _meditationDataService.AddAsync(meditationData);

            meditationDataDto = DtoConverter.GetMeditationDataDto(meditationData);

            return Ok(meditationDataDto);
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<List<MeditationDataDto>>> GetAllByAccountIdAndDate(int id, DateTime date)
        {
            VerifyAccountId(id);
            var meditationDatas = await _meditationDataService.GetAllByAccountIdAndDateAsync(id, date);

            List<MeditationDataDto> meditationDataDtos = new List<MeditationDataDto>();

            foreach (var meditationData in meditationDatas)
            {
                var meditationDataDto = DtoConverter.GetMeditationDataDto(meditationData);

                meditationDataDtos.Add(meditationDataDto);
            }

            return Ok(meditationDataDtos);
        }


        //[Authorize]
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<MeditationDataDto>> GetByMeditationDataId(int id)
        //{
        //    var meditationData = await _meditationDataService.GetByIdAsync(id);

        //    if (meditationData != null)
        //    {
        //        var meditationDataDto = DtoConverter.GetMeditationDataDto(meditationData);

        //        return Ok(meditationDataDto);
        //    }

        //    return Ok(new MeditationDataDto());
        //}


    }
}
