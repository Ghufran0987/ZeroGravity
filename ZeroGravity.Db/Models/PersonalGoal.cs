using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId))]
    public class PersonalGoal : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        public double WaterConsumption { get; set; }

        public double CalorieDrinkAlcoholConsumption { get; set; }

        public FoodAmountType BreakfastAmount { get; set; }

        public FoodAmountType LunchAmount { get; set; }

        public FoodAmountType DinnerAmount { get; set; }

        public FoodAmountType HealthySnackAmount { get; set; }

        public FoodAmountType UnhealthySnackAmount { get; set; }

        public double ActivityDuration { get; set; }

        public double FastingDuration { get; set; }

        public double MeditationDuration { get; set; }

        public double Weight { get; set; }

        public double BodyFat { get; set; }

        public double BodyMassIndex { get; set; }
    }
}