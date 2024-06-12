using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class InsightReportVideoDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int SessionId { get; set; }

        public DateTime Created { get; set; }

        public string Source { get; set; } // e.g. Synthesia

        public string SourceId { get; set; } // The id allocated to the video by the video provider, e.g. the Synthesia video id

        public string Status { get; set; }

        public string Title { get; set; }

        public string Error { get; set; }

        public Boolean Published { get; set; }
    }
}
