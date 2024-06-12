using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Validators;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalDataController : BaseController
    {
        private readonly IPersonalDataService _personalDataService;
        private readonly IAccountService _accountService;

        public PersonalDataController(IPersonalDataService personalDataService, IAccountService accountService)
        {
            _personalDataService = personalDataService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonalDataDto>> GetById(int id)
        {
            VerifyAccountId(id);
            var personalData = await _personalDataService.GetByAccounIdAsync(id);
            if (personalData == null) return null;

            var accountData = _accountService.GetById(id);
            if (personalData != null && accountData != null)
            {
                var personalDataDto = DtoConverter.GetPersonalDataDto(personalData);
                personalDataDto.FirstName = accountData.FirstName;
                personalDataDto.LastName = accountData.LastName;
                personalDataDto.UnitDisplayType = accountData.UnitDisplayType;
                personalDataDto.DateTimeDisplayType = accountData.DateTimeDisplayType;

                return Ok(personalDataDto);
            }
            return Ok(personalData);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<PersonalDataDto>> Create(PersonalDataDto personalDataDto)
        {
            VerifyAccountId(personalDataDto.AccountId);
            var personalData = DtoConverter.GetPersonalData(personalDataDto);

            personalData = await _personalDataService.AddAsync(personalData);

            personalDataDto = DtoConverter.GetPersonalDataDto(personalData);

            return Ok(personalDataDto);
        }

        [Authorize]
        [ValidateModelState]
        [HttpPut("update")]
        public async Task<ActionResult<PersonalDataDto>> Update(PersonalDataDto personalDataDto)
        {
            VerifyAccountId(personalDataDto.AccountId);
            var personalData = DtoConverter.GetPersonalData(personalDataDto);

            personalData = await _personalDataService.UpdateAsync(personalData);

            personalDataDto = DtoConverter.GetPersonalDataDto(personalData);

            return Ok(personalDataDto);
        }
    }
}