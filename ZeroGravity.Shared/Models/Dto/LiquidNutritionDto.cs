using System;
using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class LiquidNutritionDto
    {

        public LiquidNutritionDto()
        {
        }

        public int Id { get; set; }

        public int LiquidIntakeId { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public ICollection<LiquidComponentNutritionDto> LiquidComponentNutrition { get; set; }


    }
}