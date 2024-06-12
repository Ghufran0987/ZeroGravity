using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class LiquidNutrition : ModelBase
    {
        [Required]
        public int LiquidIntakeId { get; set; }
        public virtual LiquidIntake LiquidIntake { get; set; }

        public DateTime Created { get; set; }

        public int EstimatedGI { get; set; }

        public int EstimatedCarbs { get; set; }

        public int EstimatedCalories { get; set; }

        public ICollection<LiquidComponentNutrition> LiquidComponentNutrition { get; set; }

    }
}