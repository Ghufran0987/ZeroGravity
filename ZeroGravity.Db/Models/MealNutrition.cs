using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZeroGravity.Db.Models.MealIngredients;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Db.Models
{
    public class MealNutrition : ModelBase
    {
        [Required]
        public int MealDataId { get; set; }
        public virtual MealData MealData { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public ICollection<MealComponentNutrition> MealComponentNutrition { get; set; }

        public ICollection<MealFoodSwaps> MealFoodSwaps { get; set; }
    }
}