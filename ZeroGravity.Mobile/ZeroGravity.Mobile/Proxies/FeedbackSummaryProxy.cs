using System.Collections.Generic;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class FeedbackSummaryProxy : ProxyBase
    {
        private FeedbackDataProxy _activityFeedbackDataProxy;

        private FeedbackDataProxy _breakfastFeedbackDataProxy;

        private List<CoachingDataProxy> _coachingDataProxies;

        private FeedbackDataProxy _dinnerFeedbackDataProxy;

        private FeedbackDataProxy _healthySnackFeedbackDataProxy;

        private FeedbackDataProxy _lunchFeedbackDataProxy;

        private FeedbackDataProxy _meditationFeedbackDataProxy;

        private PersonalDataProxy _personalDataProxy;

        private FeedbackDataProxy _calorieDrinkFeedbackDataProxy;

        private FeedbackDataProxy _unhealthySnackFeedbackDataProxy;

        private FeedbackDataProxy _waterFeedbackDataProxy;

        public FeedbackSummaryProxy()
        {
            CoachingDataProxies = new List<CoachingDataProxy>();
        }

        public FeedbackDataProxy MeditationFeedbackDataProxy
        {
            get => _meditationFeedbackDataProxy;
            set => SetProperty(ref _meditationFeedbackDataProxy, value);
        }


        public FeedbackDataProxy BreakfastFeedbackDataProxy
        {
            get => _breakfastFeedbackDataProxy;
            set => SetProperty(ref _breakfastFeedbackDataProxy, value);
        }

        public FeedbackDataProxy LunchFeedbackDataProxy
        {
            get => _lunchFeedbackDataProxy;
            set => SetProperty(ref _lunchFeedbackDataProxy, value);
        }

        public FeedbackDataProxy DinnerFeedbackDataProxy
        {
            get => _dinnerFeedbackDataProxy;
            set => SetProperty(ref _dinnerFeedbackDataProxy, value);
        }

        public FeedbackDataProxy HealthySnackFeedbackDataProxy
        {
            get => _healthySnackFeedbackDataProxy;
            set => SetProperty(ref _healthySnackFeedbackDataProxy, value);
        }

        public FeedbackDataProxy UnhealthySnackFeedbackDataProxy
        {
            get => _unhealthySnackFeedbackDataProxy;
            set => SetProperty(ref _unhealthySnackFeedbackDataProxy, value);
        }

        public PersonalDataProxy PersonalDataProxy
        {
            get => _personalDataProxy;
            set => SetProperty(ref _personalDataProxy, value);
        }

        public FeedbackDataProxy WaterFeedbackDataProxy
        {
            get => _waterFeedbackDataProxy;
            set => SetProperty(ref _waterFeedbackDataProxy, value);
        }

        public FeedbackDataProxy CalorieDrinkFeedbackDataProxy
        {
            get => _calorieDrinkFeedbackDataProxy;
            set => SetProperty(ref _calorieDrinkFeedbackDataProxy, value);
        }

        public FeedbackDataProxy ActivityFeedbackDataProxy
        {
            get => _activityFeedbackDataProxy;
            set => SetProperty(ref _activityFeedbackDataProxy, value);
        }

        public List<CoachingDataProxy> CoachingDataProxies
        {
            get => _coachingDataProxies;
            set => SetProperty(ref _coachingDataProxies, value);
        }
    }
}