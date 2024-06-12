using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class MyDayBadgeInformationProxy : ProxyBase
    {
        private ActivityBadgeInformationProxy _activityBadgeInformationProxy;

        private LiquidIntakeBadgeInformationProxy _liquidIntakeBadgeInformationProxy;

        private MealsBadgeInformationProxy _mealsBadgeInformationProxy;

        private int _wellbeingBadgeInformation;

        public MyDayBadgeInformationProxy()
        {
            ActivityBadgeInformationProxy = new ActivityBadgeInformationProxy();
            LiquidIntakeBadgeInformationProxy = new LiquidIntakeBadgeInformationProxy();
            MealsBadgeInformationProxy = new MealsBadgeInformationProxy();
        }

        public int WellbeingBadgeInformation
        {
            get => _wellbeingBadgeInformation;
            set => SetProperty(ref _wellbeingBadgeInformation, value);
        }


        public ActivityBadgeInformationProxy ActivityBadgeInformationProxy
        {
            get => _activityBadgeInformationProxy;
            set => SetProperty(ref _activityBadgeInformationProxy, value);
        }

        public MealsBadgeInformationProxy MealsBadgeInformationProxy
        {
            get => _mealsBadgeInformationProxy;
            set => SetProperty(ref _mealsBadgeInformationProxy, value);
        }

        public LiquidIntakeBadgeInformationProxy LiquidIntakeBadgeInformationProxy
        {
            get => _liquidIntakeBadgeInformationProxy;
            set => SetProperty(ref _liquidIntakeBadgeInformationProxy, value);
        }
    }
}