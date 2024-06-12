using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class WeightDetailsDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }
        public double InitialWeight { get; set; }
        public double TargetWeight { get; set; }
        public double CurrentWeight { get; set; }
        public List<WeightDto> weights { get; set; }
    }
}
