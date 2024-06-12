using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class ActivityDataDto
    {
        public ActivityDataDto()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public int SyncId { get; set; }

        public int IntegrationId { get; set; }

        public string Name { get; set; }

        public ActivityType ActivityType { get; set; }

        //public ExerciseType? ExerciseType { get; set; }

        // In Minutes
        public double Duration { get; set; }

        public DateTime Created { get; set; }
        public ActivityIntensityType ActivityIntensityType { get; set; }
    }
}