using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("video")]
    public class VideoStreamController : BaseController
    {
        private readonly IVideoStreamService _streamContent;

        public VideoStreamController(IVideoStreamService streamContent)
        {
            _streamContent = streamContent;
        }

        //[Authorize]
        [HttpGet("{type:int}/all")]
        public async Task<ActionResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(int type)
        {
            var contentType = (StreamContentType)type;

            var content = await _streamContent.GetAvailableContentAsync(contentType);

            return Ok(content);
        }
    }
}