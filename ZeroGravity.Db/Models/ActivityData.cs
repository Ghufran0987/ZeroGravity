using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId), nameof(Created))]
    public class ActivityData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public int SyncId { get; set; }

        public int IntegrationId { get; set; }

        public string Name { get; set; }

        public ActivityType ActivityType { get; set; }

        //public ExerciseType? ExerciseType { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime Created { get; set; }

        public ActivityIntensityType ActivityIntensityType { get; set; }
    }
}