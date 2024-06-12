using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Controllers
{
    [ApiController]
    [Route("stream")]
    public class StreamController : BaseController
    {
        private readonly IStreamContentService _streamContent;

        public StreamController(IStreamContentService streamContent)
        {
            _streamContent = streamContent;
        }

        //[Authorize]
        [HttpGet("{type:int}/all")]
        public async Task<ActionResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(int type)
        {
            var contentType = (StreamContentType) type;

            var content = await _streamContent.GetAvailableContentAsync(contentType);

            return Ok(content);
        }
    }
}
