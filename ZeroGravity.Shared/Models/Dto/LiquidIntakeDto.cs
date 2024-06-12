using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class LiquidIntakeDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public LiquidType LiquidType { get; set; }

        public DateTime Created { get; set; }

        public double Amount { get; set; }
    }
}