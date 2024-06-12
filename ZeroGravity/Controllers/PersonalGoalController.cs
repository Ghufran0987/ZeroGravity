using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalGoalController : BaseController
    {
        private readonly IPersonalGoalsService _personalGoalsService;

        public PersonalGoalController(IPersonalGoalsService personalGoalsService)
        {
            _personalGoalsService = personalGoalsService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PersonalGoalDto>> GetById(int id)
        {
            VerifyAccountId(id);
            var personalGoal = await _personalGoalsService.GetByAccounIdAsync(id);

            if (personalGoal != null)
            {
                var personalGoalDto = DtoConverter.GetPersonalGoalDto(personalGoal);

                return Ok(personalGoalDto);
            }

            return Ok(new PersonalDataDto());
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<PersonalGoalDto>> Create(PersonalGoalDto personalGoalDto)
        {
            VerifyAccountId(personalGoalDto.AccountId);
            var personalGoal = DtoConverter.GetPersonalGoal(personalGoalDto);

            personalGoal = await _personalGoalsService.AddAsync(personalGoal);

            personalGoalDto = DtoConverter.GetPersonalGoalDto(personalGoal);

            return Ok(personalGoalDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<PersonalGoalDto>> Update(PersonalGoalDto personalGoalDto)
        {
            VerifyAccountId(personalGoalDto.AccountId);
            var personalGoal = DtoConverter.GetPersonalGoal(personalGoalDto);

            personalGoal = await _personalGoalsService.UpdateAsync(personalGoal);

            personalGoalDto = DtoConverter.GetPersonalGoalDto(personalGoal);

            return Ok(personalGoalDto);
        }
    }
}