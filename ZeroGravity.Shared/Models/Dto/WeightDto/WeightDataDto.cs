using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class WeightDataDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime Created { get; set; }

        public decimal Value { get; set; }

        public int WeightTrackerId { get; set; }
    }
}