using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class DietPreferencesDto
    {
        public DietPreferencesDto()
        {
            BreakfastTime = new TimeSpan(8,0,0).ToString();
            LunchTime = new TimeSpan(12,0,0).ToString();
            DinnerTime = new TimeSpan(18,0,0).ToString();
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public DietType DietType { get; set; }

        public string BreakfastTime { get; set; }

        public string LunchTime { get; set; }

        public string DinnerTime { get; set; }
    }
}