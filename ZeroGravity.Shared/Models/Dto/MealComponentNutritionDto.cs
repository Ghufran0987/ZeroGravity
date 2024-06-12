using System;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MealComponentNutritionDto
    {

        public MealComponentNutritionDto()
        {
        }

        public int Id { get; set; }

        public int MealDataId { get; set; }

        public string MealComponent { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public int AssumedPortionSize { get; set; }

        public int MealNutritionId { get; set; }

    }
}