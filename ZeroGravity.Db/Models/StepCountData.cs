using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class StepCountData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public int StepCount { get; set; }

        public DateTime TargetDate { get; set; }
    }
}