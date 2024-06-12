using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FitbitActivityDataDto
    {
        public FitbitActivityDataDto()
        {
            Activities = new List<Activity>();
        }

        public List<Activity> Activities { get; set; }

        public DateTime TargetDate { get; set; }

        public int IntegrationId { get; set; }
    }

    public class Activity
    {
        public int ActivityId { get; set; }

        public int ActivityParentId { get; set; }

        public int Calories { get; set; }

        public string Description { get; set; }

        public double Distance { get; set; }

        public int Duration { get; set; }

        public bool HastStartTime { get; set; }

        public string Name { get; set; }

        public string StartTime { get; set; }

        public int Steps { get; set; }
    }
}
