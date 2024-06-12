using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MyDayBadgeInformationDto
    {
        public LiquidIntakeBadgeInformationDto LiquidIntakeBadgeInformationDto { get; set; }

        public ActivityBadgeInformationDto ActivityBadgeInformationDto { get; set; }

        public MealsBadgeInformationDto MealsBadgeInformationDto { get; set; }

        public WellbeingType WellbeingBadgeInformation { get; set; }
    }
}