using System;
using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class WeightTrackerDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }

        public decimal InitialWeight { get; set; }
        public decimal TargetWeight { get; set; }
        public decimal CurrentWeight { get; set; }

        public ICollection<WeightDataDto> Weights { get; set; }
    }
}