using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Db.Models;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FastingDataController : BaseController
    {
        private readonly IFastingDataService _fastingDataService;

        public FastingDataController(IFastingDataService fastingDataService)
        {
            _fastingDataService = fastingDataService;
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}")]
        public async Task<ActionResult<FastingDataDto>> GetByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var fastingData = await _fastingDataService.GetByAccountIdAsync(id, date);

            if (fastingData != null)
            {
                var fastingDataDto = DtoConverter.GetFastingDataDto(fastingData);

                return Ok(fastingDataDto);
            }

            return NotFound(null);
        }

        [Authorize]
        [HttpGet("{id:int}/{dateStart:DateTime}/{dateEnd:DateTime}")]
        public async Task<ActionResult<FastingDataDto>> GetByAccountId(int id, DateTime dateStart, DateTime dateEnd)
        {
            VerifyAccountId(id);
            List<FastingData> fastingData = await _fastingDataService.GetByAccountIdAndDateRangeAsync(id, dateStart, dateEnd);
            List<FastingDataDto> result = new List<FastingDataDto>();
            foreach(FastingData fd in fastingData)
            {
                result.Add(DtoConverter.GetFastingDataDto(fd));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:int}/{date:DateTime}/active")]
        public async Task<ActionResult<IEnumerable<FastingDataDto>>> GetActivesByAccountId(int id, DateTime date)
        {
            VerifyAccountId(id);
            var fastingDataList = await _fastingDataService.GetActivesByAccountIdAsync(id, date);

            if (fastingDataList != null)
            {
                var dtos = new List<FastingDataDto>();
                foreach (var fastingData in fastingDataList)
                {
                    var fastingDataDto = DtoConverter.GetFastingDataDto(fastingData);
                    dtos.Add(fastingDataDto);
                }

                return Ok(dtos);
            }

            return BadRequest(new List<FastingDataDto>());
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<FastingDataDto>> Create(FastingDataDto fastingDataDto)
        {
            VerifyAccountId(fastingDataDto?.AccountId);
            var fastingData = DtoConverter.GetFastingData(fastingDataDto);

            fastingData = await _fastingDataService.AddAsync(fastingData);

            fastingDataDto = DtoConverter.GetFastingDataDto(fastingData);

            return Ok(fastingDataDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<FastingDataDto>> Update(FastingDataDto fastingDataDto)
        {
            VerifyAccountId(fastingDataDto?.AccountId);
            var fastingData = DtoConverter.GetFastingData(fastingDataDto);

            fastingData = await _fastingDataService.UpdateAsync(fastingData);

            fastingDataDto = DtoConverter.GetFastingDataDto(fastingData);

            return Ok(fastingDataDto);
        }
    }
}