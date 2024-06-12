using System;
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
    public class TestModeController : BaseController
    {
        private readonly ITestModeService _testModeService;

        public TestModeController(ITestModeService testModeService)
        {
            _testModeService = testModeService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<bool> Get()
        {
            var isTestModeActive =  _testModeService.GetTestModeInfo();

            return Ok(isTestModeActive);
        }

        [Authorize]
        [HttpPut("update")]
        public ActionResult Update(bool isActive)
        {
            _testModeService.SetTestMode(isActive);

            return Ok();
        }
    }
}
