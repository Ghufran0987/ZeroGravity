using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntegrationDataController : BaseController
    {
        private readonly IIntegrationDataService _integrationDataService;

        public IntegrationDataController(IIntegrationDataService integrationDataService)
        {
            _integrationDataService = integrationDataService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<List<IntegrationDataDto>>> GetSupportedIntegrations(int id)
        {

            VerifyAccountId(id);
            var integrationDatas = await _integrationDataService.GetAvailableIntegrationsAsync();

            var linkedIntegrations = await _integrationDataService.GetLinkedIntegrationsByAccountIdAsync(id);

            var integrationDataDtos = new List<IntegrationDataDto>();

            foreach (var integrationData in integrationDatas)
            {
                var integrationDataDto = DtoConverter.GetIntegrationDataDto(integrationData);

                if (linkedIntegrations.Any(_ => _.IntegrationId == integrationData.Id))
                    integrationDataDto.IsLinked = true;

                integrationDataDtos.Add(integrationDataDto);
            }

            return Ok(integrationDataDtos);
        }

        //[Authorize]
        //[HttpGet("tokendata/{id:int}/{integrationId:int}")]
        //public async Task<ActionResult<LinkedIntegrationDto>> GetLinkedIntegrationData(int id, int integrationId)
        //{
        //    var linkedIntegration =
        //        await _integrationDataService.GetLinkedIntegrationByIntegrationIdAsync(id, integrationId);


        //    if (linkedIntegration != null)
        //    {
        //        var linkedIntegrationDto = DtoConverter.GetLinkedIntegrationDto(linkedIntegration);

        //        return Ok(linkedIntegrationDto);
        //    }

        //    return Ok(new LinkedIntegrationDto());
        //}

        [Authorize]
        [HttpGet("linked/{id:int}")]
        public async Task<ActionResult<List<IntegrationDataDto>>> GetLinkedIntegrations(int id)
        {
            VerifyAccountId(id);

            var integrationDatas = await _integrationDataService.GetAvailableIntegrationsAsync();

            var linkedIntegrations = await _integrationDataService.GetLinkedIntegrationsByAccountIdAsync(id);

            var integrationDataDtos = new List<IntegrationDataDto>();

            foreach (var linkedIntegration in linkedIntegrations)
            {
                var integrationData = integrationDatas.FirstOrDefault(_ => _.Id == linkedIntegration.IntegrationId);

                if (integrationData != null)
                {
                    var integrationDataDto = DtoConverter.GetIntegrationDataDto(integrationData);

                    integrationDataDto.IsLinked = true;

                    integrationDataDtos.Add(integrationDataDto);
                }
            }

            return Ok(integrationDataDtos);
        }

        [Authorize]
        [HttpDelete("{id:int}/{integrationid:int}")]
        public async Task<ActionResult> DeleteLinkedIntegration(int id, int integrationid)
        {
            VerifyAccountId(id);
            await _integrationDataService.DeleteAsync(id, integrationid);

            return Ok();
        }
    }
}