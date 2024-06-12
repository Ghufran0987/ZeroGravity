using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FitbitController : BaseController
    {
        private readonly IFitbitClientService _fitbitClientService;

        public FitbitController(IFitbitClientService fitbitClientService)
        {
            _fitbitClientService = fitbitClientService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FitbitAccountDto>> GetById(int id)
        {
            VerifyAccountId(id);
            var fitbitAccountDto = await _fitbitClientService.GetFitbitAuthenticationInfo(id);

            if(fitbitAccountDto != null)
            {
                return Ok(fitbitAccountDto);
            }

            return Ok(new FitbitAccountDto());
        }

        //[Authorize]
        [HttpGet("callback")]
        public async Task<ActionResult> Callback([FromQuery]FitbitCallbackDto fitbitCallbackParams)
        {
            if (!string.IsNullOrEmpty(fitbitCallbackParams.Code))
            {
                var isSuccessful = await _fitbitClientService.GetFitbitUserTokenByCode(fitbitCallbackParams);

                if (isSuccessful)
                {
                    return RedirectToPage("/FitBitCallback");
                }

                return BadRequest();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet("activities/{id:int}/{integrationId:int}/{date:DateTime}")]
        public async Task<ActionResult<FitbitActivityDataDto>> GetLinkedIntegrationData(int id, int integrationId, DateTime date)
        {
            VerifyAccountId(id);

            if (await _fitbitClientService.RefreshFitbitToken(id, integrationId))
            {
                var linkedIntegrationDto =
                    await _fitbitClientService.GetFitbitActivitiesAsync(id, integrationId, date);

                if (linkedIntegrationDto != null)
                {
                    return Ok(linkedIntegrationDto);
                }
            }

            return Ok(new LinkedIntegrationDto());
        }
    }

}
