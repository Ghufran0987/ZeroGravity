using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class StepCountDataDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int StepCount { get; set; }

        public DateTime TargetDate { get; set; }
    }
}