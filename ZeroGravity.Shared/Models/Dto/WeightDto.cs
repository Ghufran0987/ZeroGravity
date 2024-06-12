using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class WeightDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public double Value { get; set; }
        public int WeightTrackerId { get; set; }

    }
}
