using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionDataController : BaseController
    {
        private readonly IQuestionDataService _questionDataService;

        public QuestionDataController(IQuestionDataService questionDataService)
        {
            _questionDataService = questionDataService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<QuestionDto>> AddAsync(QuestionDto questionDto, bool saveChanges = true)
        {
            var data = DtoConverter.GetQuestionData(questionDto);
            var result = await _questionDataService.AddAsync(data, saveChanges);
            var dto = DtoConverter.GetQuestionDto(result);
            return dto;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestionDto>> GetByIdAsync(int id)
        {
            var questionData = await _questionDataService.GetByIdAsync(id);
            if (questionData != null)
            {
                QuestionDto dto = DtoConverter.GetQuestionDto(questionData);
                return Ok(dto);
            }
            return Ok(new QuestionDto());
        }

        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsAsync()
        {
            var result = await _questionDataService.GetQuestionsAsync();
            var retResult = new List<QuestionDto>();
            foreach (var data in result)
            {
                var dto = DtoConverter.GetQuestionDto(data);
                retResult.Add(dto);
            }
            return retResult;
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<QuestionDto>> UpdateAsync(QuestionDto questionDto, bool saveChanges = true)
        {
            var data = DtoConverter.GetQuestionData(questionDto);
            var result = await _questionDataService.UpdateAsync(data, saveChanges);
            var dto = DtoConverter.GetQuestionDto(result);
            return dto;
        }
    }
}