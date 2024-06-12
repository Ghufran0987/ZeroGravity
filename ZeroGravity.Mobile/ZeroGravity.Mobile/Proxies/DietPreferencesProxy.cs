using System;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class DietPreferencesProxy : ProxyBase
    {
        private TimeSpan _breakfastTime;
        private TimeSpan _dinnerTime;
        private TimeSpan _lunchTime;
        private DietType _dietType;

        public int AccountId { get; set; }

        public DietType DietType
        {
            get => _dietType;
            set => SetProperty(ref _dietType, value);
        }

        public TimeSpan BreakfastTime
        {
            get => _breakfastTime;
            set => SetProperty(ref _breakfastTime, value);
        }

        public TimeSpan LunchTime
        {
            get => _lunchTime;
            set => SetProperty(ref _lunchTime, value);
        }

        public TimeSpan DinnerTime
        {
            get => _dinnerTime;
            set => SetProperty(ref _dinnerTime, value);
        }
    }
}