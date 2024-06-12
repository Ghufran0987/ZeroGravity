using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId)), Index(nameof(AccountId), nameof(Created)), Index(nameof(AccountId), nameof(Created), nameof(Completed))]
    public class WeightTracker : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InitialWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TargetWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentWeight { get; set; }

        public ICollection<WeightData> Weights { get; set; }
    }

    [Index(nameof(AccountId), nameof(Created), nameof(WeightTrackerId))]
    public class WeightData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public DateTime Created { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        public int WeightTrackerId { get; set; }
        public WeightTracker WeightTracker { get; set; }
    }
}