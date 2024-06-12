using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DietPreferenceController : BaseController
    {
        private readonly IDietPreferenceService _dietPreferenceService;

        public DietPreferenceController(IDietPreferenceService dietPreferenceService)
        {
            _dietPreferenceService = dietPreferenceService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DietPreferencesDto>> GetById(int id)
        {
            VerifyAccountId(id);
            var dietPreference = await _dietPreferenceService.GetByAccounId(id);

            if (dietPreference != null)
            {
                var dietPreferencesDto = DtoConverter.GetDietPreferencesDto(dietPreference);

                return Ok(dietPreferencesDto);
            }

            return Ok(new DietPreferencesDto());
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<DietPreferencesDto>> Create(DietPreferencesDto dietPreferencesDto)
        {
            VerifyAccountId(dietPreferencesDto?.AccountId);
            var dietPreferences = DtoConverter.GetDietPreferences(dietPreferencesDto);

            dietPreferences = await _dietPreferenceService.AddAsync(dietPreferences);

            dietPreferencesDto = DtoConverter.GetDietPreferencesDto(dietPreferences);

            return Ok(dietPreferencesDto);
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<DietPreferencesDto>> Update(DietPreferencesDto dietPreferencesDto)
        {
            VerifyAccountId(dietPreferencesDto?.AccountId);
            var dietPreferences = DtoConverter.GetDietPreferences(dietPreferencesDto);

            dietPreferences = await _dietPreferenceService.UpdateAsync(dietPreferences);

            dietPreferencesDto = DtoConverter.GetDietPreferencesDto(dietPreferences);

            return Ok(dietPreferencesDto);
        }
    }
}