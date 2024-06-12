using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    [Index(nameof(AccountId), nameof(StartTime), nameof(EndTime))]
    public class SugarBeatEatingSession : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MetabolicScore { get; set; }

        public bool IsCompleted { get; set; }
    }
}