using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class MealsBadgeInformationProxy : ProxyBase
    {
        private FoodAmountType _breakfastAmount;

        private FoodAmountType _dinnerAmount;

        private int _healthySnackNumber;

        private FoodAmountType _lunchAmount;

        private int _unhealthySnackNumber;
        private int _totalAmount;

        public int HealthySnackNumber
        {
            get => _healthySnackNumber;
            set => SetProperty(ref _healthySnackNumber, value);
        }

        public int UnhealthySnackNumber
        {
            get => _unhealthySnackNumber;
            set => SetProperty(ref _unhealthySnackNumber, value);
        }

        public FoodAmountType BreakfastAmount
        {
            get => _breakfastAmount;
            set => SetProperty(ref _breakfastAmount, value);
        }

        public FoodAmountType LunchAmount
        {
            get => _lunchAmount;
            set => SetProperty(ref _lunchAmount, value);
        }

        public FoodAmountType DinnerAmount
        {
            get => _dinnerAmount;
            set => SetProperty(ref _dinnerAmount, value);
        }

        public int TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }
    }
}