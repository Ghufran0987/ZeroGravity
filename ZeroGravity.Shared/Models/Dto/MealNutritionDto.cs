using System;
using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MealNutritionDto
    {

        public MealNutritionDto()
        {
        }

        public int Id { get; set; }

        public int MealDataId { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public ICollection<MealComponentNutritionDto> MealComponentNutrition { get; set; }

        public ICollection<MealFoodSwapsDto> MealFoodSwaps { get; set; }

    }
}