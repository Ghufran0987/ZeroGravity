using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class FeedbackDataProxy : ProxyBase
    {
        private double _actualValue;

        private FeedbackRangeType _feedbackRange;

        private FeedbackState _feedbackState;

        private FeedbackType _feedbackType;

        private ImageSource _image;

        private string _recommendation;

        private double _recommendedValue;

        private int _score;

        private string _title;

        public FeedbackRangeType FeedbackRange
        {
            get => _feedbackRange;
            set => SetProperty(ref _feedbackRange, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Recommendation
        {
            get => _recommendation;
            set => SetProperty(ref _recommendation, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public double RecommendedValue
        {
            get => _recommendedValue;
            set => SetProperty(ref _recommendedValue, value);
        }

        public double ActualValue
        {
            get => _actualValue;
            set => SetProperty(ref _actualValue, value);
        }

        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }


        public FeedbackType FeedbackType
        {
            get => _feedbackType;
            set => SetProperty(ref _feedbackType, value);
        }

        public FeedbackState FeedbackState
        {
            get => _feedbackState;
            set => SetProperty(ref _feedbackState, value);
        }
    }
}