using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class MealFoodSwaps : ModelBase
    {
        [Required]
        public int MealDataId { get; set; }

        public DateTime Created { get; set; }

        public string SwapDescription { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public int MealNutritionId { get; set; }
        public virtual MealNutrition MealNutrition { get; set; }
    }
}