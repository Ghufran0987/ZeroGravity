using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FilterMealDataDto
    {
        public int AccountId { get; set; }
        public MealSlotType? SlotType { get; set; }
        public FoodAmountType? FoodAmountType { get; set; }
        public bool WithIngredients { get; set; }
        public DateTime? DateTime { get; set; }

    }
}