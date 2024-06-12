using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId)), Index(nameof(SessionId))]
    public class InsightReportVideo : ModelBase
    {
        [Required] public int AccountId { get; set; }

        [Required] public int SessionId { get; set; }

        [Required] public DateTime Created { get; set; }

        [Required] public string Source { get; set; } // e.g. Synthesia

        public string SourceId { get; set; } // The id allocated to the video by the video provider, e.g. the Synthesia video id

        public string Status { get; set; }

        public string Title { get; set; }

        public string Error { get; set; }

        public Boolean Published { get; set; }
    }
}