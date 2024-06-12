using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class GlucoseTrendDto
    {

        public GlucoseTrendDto()
        {
        }

        public int Id { get; set; }

        public int SessionId { get; set; }

        public int AccountId { get; set; }

        public string Trend { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ActivityDataDto AssociatedActivityData { get; set; }

        public MealDataDto AssociatedMealData { get; set; }

    }
}