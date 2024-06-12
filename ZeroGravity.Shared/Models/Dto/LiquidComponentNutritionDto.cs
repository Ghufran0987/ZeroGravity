using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class LiquidComponentNutritionDto
    {

        public LiquidComponentNutritionDto()
        {
        }

        public int Id { get; set; }

        public int LiquidIntakeId { get; set; }

        public string LiquidComponent { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public int AssumedPortionSize { get; set; }

        public int LiquidNutritionId { get; set; }

    }
}