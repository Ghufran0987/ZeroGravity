using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class LiquidIntakeBadgeInformationProxy : ProxyBase
    {
        private int _waterIntakeNumber;

        public int WaterIntakeNumber
        {
            get => _waterIntakeNumber;
            set => SetProperty(ref _waterIntakeNumber, value);
        }

        private int _calorieDrinksAlcoholNumber;

        public int CalorieDrinksAlcoholNumber
        {
            get => _calorieDrinksAlcoholNumber;
            set => SetProperty(ref _calorieDrinksAlcoholNumber, value);
        }
    }
}