using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(SessionId)), Index(nameof(AccountId), nameof(SessionId))]
    public class GlucoseTrend : ModelBase
    {
        [Required]
        public int SessionId { get; set; }
        public virtual SugarBeatSessionData Session { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public string Trend { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public int? AssociatedMealDataId { get; set; }
        public virtual MealData AssociatedMealData { get; set; }

        public int? AssociatedActivityDataId { get; set; }
        public virtual ActivityData AssociatedActivityData { get; set; }
    }
}