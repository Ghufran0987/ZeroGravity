using System;
using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MealFoodSwapsDto
    {

        public MealFoodSwapsDto()
        {
        }

        public int Id { get; set; }

        public int MealDataId { get; set; }

        public DateTime Created { get; set; }

        public string SwapDescription { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public int MealNutritionId { get; set; }

    }
}