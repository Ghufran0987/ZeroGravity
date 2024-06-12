using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MealsBadgeInformationDto
    {
        public int HealthySnackNumber { get; set; }
        public int UnhealthySnackNumber { get; set; }
        public FoodAmountType BreakfastAmount { get; set; }
        public FoodAmountType LunchAmount { get; set; }
        public FoodAmountType DinnerAmount { get; set; }
        public int TotalAmount { get; set; }
    }
}