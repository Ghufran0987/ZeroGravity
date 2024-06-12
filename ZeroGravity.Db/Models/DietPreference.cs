using System;
using System.ComponentModel.DataAnnotations;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    public class DietPreference : ModelBase
    {
        [Required] 
        public int AccountId { get; set; }

        public DietType DietType { get; set; }

        public TimeSpan BreakfastTime { get; set; }

        public TimeSpan LunchTime { get; set; }

        public TimeSpan DinnerTime { get; set; }
    }
}