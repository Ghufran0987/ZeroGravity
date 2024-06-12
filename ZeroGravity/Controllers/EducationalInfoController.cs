using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EducationalInfoController : BaseController
    {
        private readonly IEducationalInfoDataService _educationalInfoDataService;

        public EducationalInfoController(IEducationalInfoDataService educationalInfoDataService)
        {
            _educationalInfoDataService = educationalInfoDataService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EducationalInfoDto>> GetById(int id)
        {
            var eduInfoData = await _educationalInfoDataService.GetByIdAsync(id);
            if (eduInfoData != null)
            {
                var dto = DtoConverter.GetEducationalInfoDto(eduInfoData);
                return Ok(dto);
            }
            return NotFound(null);
        }

        [Authorize]
        [HttpGet("{category}")]
        public async Task<ActionResult<EducationalInfoDto>> GetRandomByCategory(string category)
        {
            var data = await _educationalInfoDataService.GetRandomByCategoryAsync(category);
            if (data != null)
            {
                var dto = DtoConverter.GetEducationalInfoDto(data);
                return Ok(dto);
            }
            return NotFound(null);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<EducationalInfoDto>> Create(EducationalInfoDto eduInfoDto)
        {
            var eduInfoData = DtoConverter.GetEducationalInfoData(eduInfoDto);
            eduInfoData = await _educationalInfoDataService.AddAsync(eduInfoData);
            eduInfoDto = DtoConverter.GetEducationalInfoDto(eduInfoData);
            return Ok(eduInfoDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<EducationalInfoDto>> Update(EducationalInfoDto eduInfoDto)
        {
            var eduInfoData = DtoConverter.GetEducationalInfoData(eduInfoDto);
            eduInfoData = await _educationalInfoDataService.UpdateAsync(eduInfoData);
            eduInfoDto = DtoConverter.GetEducationalInfoDto(eduInfoData);
            return Ok(eduInfoDto);
        }
    }
}