using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBeatSettingDataController : BaseController
    {
        private readonly ISugarBeatSettingDataService _sugarBeatDeviceSettingDataService;

        public SugarBeatSettingDataController(ISugarBeatSettingDataService sugarBeatSettingDataService)
        {
            _sugarBeatDeviceSettingDataService = sugarBeatSettingDataService;
        }

        [HttpPost("create")]
        public async Task<SugarBeatSettingDto> AddAsync(SugarBeatSettingDto settingDto, bool saveChanges = true)
        {
            var result = await _sugarBeatDeviceSettingDataService.AddAsync(DtoConverter.GetSugarBeatSttingData(settingDto), saveChanges);
            if (result == null) return null;
            var sugarBeatSettingDto = DtoConverter.GetSugarBeatSettingDto(result);
            return sugarBeatSettingDto;
        }

        [HttpPut("update")]
        public async Task<SugarBeatSettingDto> UpdateAsync(SugarBeatSettingDto settingDto, bool saveChanges = true)
        {
            var session = await _sugarBeatDeviceSettingDataService.GetByAccountIdAsync(settingDto.AccountId);
            if (session != null)
            {
                var result = await _sugarBeatDeviceSettingDataService.UpdateAsync(DtoConverter.GetSugarBeatSttingData(settingDto), saveChanges);
                return DtoConverter.GetSugarBeatSettingDto(result);
            }
            return null;
        }

        [HttpGet("{accountId:int}")]
        public async Task<SugarBeatSettingDto> GetByAccountIdAsync(int accountId)
        {
            var result = await _sugarBeatDeviceSettingDataService.GetByAccountIdAsync(accountId);
            if (result == null) return null;
            return DtoConverter.GetSugarBeatSettingDto(result);
        }
    }
}