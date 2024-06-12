using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class ActivityBadgeInformationProxy : ProxyBase
    {
        private int _dayToDayNumber;

        private int _exerciseNumber;

        public int DayToDayNumber
        {
            get => _dayToDayNumber;
            set => SetProperty(ref _dayToDayNumber, value);
        }

        public int ExerciseNumber
        {
            get => _exerciseNumber;
            set => SetProperty(ref _exerciseNumber, value);
        }
    }
}