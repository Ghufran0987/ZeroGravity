using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FeedbackDataDto
    {
        public FeedbackType FeedbackType { get; set; }

        public double ActualValue { get; set; }

        public double RecommendedValue { get; set; }
    }
}