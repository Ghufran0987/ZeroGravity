using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Services;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserQueryController : BaseController
    {
        private readonly IAccountService _accountService;

        public UserQueryController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<DietPreferencesDto>> Create(UserQueryDataDto model)
        {
            VerifyAccountId(model?.AccountId);
            var userQueryData = DtoConverter.GetUserQueryData(model);

            userQueryData = await _accountService.StoreUserQuery(userQueryData);

            model = DtoConverter.GetUserQueryDataDto(userQueryData);

            return Ok(model);
        }
    }
}

