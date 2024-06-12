using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FeedbackSummaryDto
    {
        public PersonalDataDto PersonalDataDto { get; set; }


        public FeedbackDataDto WaterFeedbackDataDto { get; set; }

        public FeedbackDataDto CalorieDrinkFeedbackDataDto { get; set; }


        public FeedbackDataDto ActivityFeedbackDataDto { get; set; }


        public FeedbackDataDto BreakfastFeedbackDataDto { get; set; }

        public FeedbackDataDto LunchFeedbackDataDto { get; set; }

        public FeedbackDataDto DinnerFeedbackDataDto { get; set; }


        public FeedbackDataDto HealthyFeedbackDataDto { get; set; }

        public FeedbackDataDto UnhealthyFeedbackDataDto { get; set; }


        public FeedbackDataDto MeditationDataDto { get; set; }


        public List<CoachingDataDto> CoachingDataDtos { get; set; }
    }
}
