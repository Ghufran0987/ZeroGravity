using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class LiquidComponentNutrition : ModelBase
    {
        [Required]
        public int LiquidIntakeId { get; set; }

        public string LiquidComponent { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public int AssumedPortionSize { get; set; }

        public int LiquidNutritionId { get; set; }
        public virtual LiquidNutrition LiquidNutrition { get; set; }
    }
}