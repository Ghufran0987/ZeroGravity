using System;
using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class StreamContentDto
    {
        public StreamContentDto()
        {
            Categories = new List<string>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string VideoId { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime Published { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string VideoPlayerId { get; set; }
        
        public int Duration { get; set; }
    }
}