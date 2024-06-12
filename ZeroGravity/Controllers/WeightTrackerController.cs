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
    public class WeightTrackerController : BaseController
    {
        private readonly IWeightTrackerDataService _weightTrackerDataService;

        public WeightTrackerController(IWeightTrackerDataService weightTrackerDataService)
        {
            _weightTrackerDataService = weightTrackerDataService;
        }

        [HttpPost("create")]
        public async Task<WeightTrackerDto> AddAsync(WeightTrackerDto dto, bool saveChanges = true)
        {
            var data = DtoConverter.GetWeightTrackerData(dto);
            var result = await _weightTrackerDataService.AddAsync(data, saveChanges);
            if (result == null) return null;
            var resultDto = DtoConverter.GetWeightTrackerDto(result);
            return resultDto;
        }

        [HttpPost("WeightData/create")]
        public async Task<WeightDataDto> AddWeightAsync(WeightDataDto dto, bool saveChanges = true)
        {
            // TODO Check for duplicate
            var data = DtoConverter.GetWeightData(dto);
            var result = await _weightTrackerDataService.AddWeightAsync(data, saveChanges);
            if (result == null) return null;
            var resultDto = DtoConverter.GetWeightDto(result);
            return resultDto;
        }

        [HttpPut("update")]
        public async Task<WeightTrackerDto> UpdateAsync(WeightTrackerDto dto, bool saveChanges = true)
        {
            var data = DtoConverter.GetWeightTrackerData(dto);
            var result = await _weightTrackerDataService.UpdateAsync(data, saveChanges);
            if (result == null) return null;
            var resultDto = DtoConverter.GetWeightTrackerDto(result);
            return resultDto;
        }

        [HttpGet("{id:int}")]
        public async Task<WeightTrackerDto> GetByIdAsync(int id)
        {
            var result = await _weightTrackerDataService.GetByIdAsync(id);
            if (result == null) return null;
            var resultDto = DtoConverter.GetWeightTrackerDto(result);
            return resultDto;
        }

        [HttpGet("Current/{accountId:int}")]
        public async Task<WeightTrackerDto> GetCurrentWeightTrackerAsync(int accountId)
        {
            var result = await _weightTrackerDataService.GetCurrentWeightTrackerAsync(accountId);
            if (result == null) return null;
            var resultDto = DtoConverter.GetWeightTrackerDto(result);
            return resultDto;
        }

        [HttpGet("GetAll/{accountId:int}")]
        public async Task<List<WeightTrackerDto>> GetAll(int accountId)
        {
            var retResult = new List<WeightTrackerDto>();
            var result = await _weightTrackerDataService.GetAll(accountId);
            if (result == null) return null;

            foreach (var data in result)
            {
                var resultDto = DtoConverter.GetWeightTrackerDto(data);
                retResult.Add(resultDto);
            }
            return retResult;
        }
    }
}