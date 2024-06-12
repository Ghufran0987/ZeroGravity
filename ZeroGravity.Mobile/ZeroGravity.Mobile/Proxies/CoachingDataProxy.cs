using System;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class CoachingDataProxy : ProxyBase
    {
        private CoachingType _coachingType;

        private string _description;

        private string _iconString;

        private string _title;

        public CoachingDataProxy(CoachingType coachingType)
        {
            SetCoachingValues(coachingType);
        }

        public CoachingType CoachingType
        {
            get => _coachingType;
            set => SetProperty(ref _coachingType, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string IconString
        {
            get => _iconString;
            set => SetProperty(ref _iconString, value);
        }

        private void SetCoachingValues(CoachingType coachingType)
        {
            switch (coachingType)
            {
                case CoachingType.Nutrition:

                    Title = AppResources.Feedback_NutritionTitle;
                    Description = AppResources.Feedback_NutritionDescription;
                    IconString = "\uf2e6";

                    break;
                case CoachingType.Personal:

                    Title = AppResources.Feedback_PersonalTrainingTitle;
                    Description = AppResources.Feedback_PersonalTrainingDescription;
                    IconString = "\uf460";

                    break;
                case CoachingType.Mental:

                    Title = AppResources.Feedback_MentalTrainingTitle;
                    Description = AppResources.Feedback_MentalTrainingDescription;
                    IconString = "\uf5dc";

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coachingType), coachingType, null);
            }
        }
    }
}