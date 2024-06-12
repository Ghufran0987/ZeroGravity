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
    public class InsightReportController : BaseController
    {
        private readonly IInsightReportService _service;

        public InsightReportController(IInsightReportService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("{sessionId:int}")]
        public async Task<ActionResult<List<InsightReportVideoDto>>> GetInsightReportVideoBySessionId(int sessionId)
        {
            var insightReportVideoDtos = new List<InsightReportVideoDto>();
            var insightReportVideos = await _service.GetInsightReportVideoBySessionIdAsync(sessionId);

            if (insightReportVideos == null)
            {
                return null;
            }

            foreach (var insightReportVideo in insightReportVideos)
            {
                var insightReportVideoDto = DtoConverter.GetInsightReportVideoDto(insightReportVideo);
                insightReportVideoDtos.Add(insightReportVideoDto);
            }

            return insightReportVideoDtos;
        }
    }
}