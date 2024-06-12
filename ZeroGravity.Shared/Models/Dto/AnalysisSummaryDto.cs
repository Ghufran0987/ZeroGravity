using System.Collections.Generic;

namespace ZeroGravity.Shared.Models.Dto
{
    public class AnalysisSummaryDto
    {
        public List<LiquidIntakeDto> LiquidIntakeDtos { get; set; }
        public List<MealDataDto> MealDataDtos { get; set; }
        public List<ActivityDataDto> ActivityDataDtos { get; set; }
        public PersonalGoalDto PersonalGoalDto { get; set; }

    }
}