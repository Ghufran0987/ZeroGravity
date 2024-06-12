using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FastingSettingController : BaseController
    {
        private readonly IFastingSettingsService _fastingSettingsService;

        public FastingSettingController(IFastingSettingsService fastingSettingsService)
        {
            _fastingSettingsService = fastingSettingsService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FastingSettingDto>> GetByAccountId(int id)
        {
            VerifyAccountId(id);
            var fastingSetting = await _fastingSettingsService.GetByAccounIdAsync(id);

            if (fastingSetting != null)
            {
                var fastingSettingDto = DtoConverter.GetFastingSettingDto(fastingSetting);

                return Ok(fastingSettingDto);
            }

            return Ok(new FastingSettingDto());
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<FastingSettingDto>> Create(FastingSettingDto fastingSettingDto)
        {
            VerifyAccountId(fastingSettingDto.AccountId);
            var fastingSetting = DtoConverter.GetFastingSetting(fastingSettingDto);

            fastingSetting = await _fastingSettingsService.AddAsync(fastingSetting);

            fastingSettingDto = DtoConverter.GetFastingSettingDto(fastingSetting);

            return Ok(fastingSettingDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<FastingSettingDto>> Update(FastingSettingDto fastingSettingDto)
        {
            VerifyAccountId(fastingSettingDto.AccountId);
            var fastingSetting = DtoConverter.GetFastingSetting(fastingSettingDto);

            fastingSetting = await _fastingSettingsService.UpdateAsync(fastingSetting);

            fastingSettingDto = DtoConverter.GetFastingSettingDto(fastingSetting);

            return Ok(fastingSettingDto);
        }
    }
}