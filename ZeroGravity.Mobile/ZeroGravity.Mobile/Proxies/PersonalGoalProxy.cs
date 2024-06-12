using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class PersonalGoalProxy : ProxyBase
    {
        private int _accountId;

        private int _breakfastAmount;

        private int _dinnerAmount;

        private int _lunchAmount;

        private double _waterConsumption;

        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        public double WaterConsumption
        {
            get => _waterConsumption;
            set => SetProperty(ref _waterConsumption, value);
        }

        private double _calorieDrinkConsumption;

        public double CalorieDrinkConsumption
        {
            get => _calorieDrinkConsumption;
            set => SetProperty(ref _calorieDrinkConsumption, value);
        }

        public int BreakfastAmount
        {
            get => _breakfastAmount;
            set => SetProperty(ref _breakfastAmount, value);
        }

        public int LunchAmount
        {
            get => _lunchAmount;
            set => SetProperty(ref _lunchAmount, value);
        }

        public int DinnerAmount
        {
            get => _dinnerAmount;
            set => SetProperty(ref _dinnerAmount, value);
        }

        private int _healthySnackAmount;

        public int HealthySnackAmount
        {
            get => _healthySnackAmount;
            set => SetProperty(ref _healthySnackAmount, value);
        }

        private int _unhealthySnackAmount;

        public int UnhealthySnackAmount
        {
            get => _unhealthySnackAmount;
            set => SetProperty(ref _unhealthySnackAmount, value);
        }

        private double _activityDuration;

        public double ActivityDuration
        {
            get => _activityDuration;
            set => SetProperty(ref _activityDuration, value);
        }

        private double _fsatingDuration;

        public double FastingDuration
        {
            get => _fsatingDuration;
            set => SetProperty(ref _fsatingDuration, value);
        }


        private double _bodyMassIndex;

        public double BodyMassIndex
        {
            get => _bodyMassIndex;
            set => SetProperty(ref _bodyMassIndex, value);
        }

        private double _bodyFat;

        public double BodyFat
        {
            get => _bodyFat;
            set => SetProperty(ref _bodyFat, value);
        }


        private double _meditationDuration;

        public double MeditationDuration
        {
            get => _meditationDuration;
            set => SetProperty(ref _meditationDuration, value);
        }

        private double _weight;

        public double Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

      
    }
}