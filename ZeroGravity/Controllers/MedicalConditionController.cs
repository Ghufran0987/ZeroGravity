using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicalConditionController : BaseController
    {
        private readonly IMedicalConditionService _medicalConditionService;

        public MedicalConditionController(IMedicalConditionService medicalConditionService)
        {
            _medicalConditionService = medicalConditionService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicalConditionDto>> GetById(int id)
        {
            VerifyAccountId(id);
            var medicalCondition = await _medicalConditionService.GetByAccounId(id);

            if (medicalCondition != null)
            {
                var medicalConditionDto = DtoConverter.GetMedicalConditionDto(medicalCondition);

                return Ok(medicalConditionDto);
            }

            return Ok(new MedicalConditionDto());
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<MedicalConditionDto>> Create(MedicalConditionDto medicalConditionDto)
        {
            VerifyAccountId(medicalConditionDto.AccountId);
            var medicalCondition = DtoConverter.GetMedicalCondition(medicalConditionDto);

            medicalCondition = await _medicalConditionService.AddAsync(medicalCondition);

            medicalConditionDto = DtoConverter.GetMedicalConditionDto(medicalCondition);

            return Ok(medicalConditionDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<MedicalConditionDto>> Update(MedicalConditionDto medicalConditionDto)
        {
            VerifyAccountId(medicalConditionDto.AccountId);
            var medicalCondition = DtoConverter.GetMedicalCondition(medicalConditionDto);

            medicalCondition = await _medicalConditionService.UpdateAsync(medicalCondition);

            medicalConditionDto = DtoConverter.GetMedicalConditionDto(medicalCondition);

            return Ok(medicalConditionDto);
        }
    }
}