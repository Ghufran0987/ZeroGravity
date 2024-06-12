using System;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class DayToDayActivityProxy : ActivityProxyBase
    {
        private DateTime _dayToDayDateTime;

        private TimeSpan _dayToDayTime;

        public DayToDayActivityProxy()
        {
            DailyThreshold = ActivityConstants.DayToDayDurationThreshold;
        }

        public int AccountId { get; set; }

        public ActivityType ActivityType => ActivityType.DayToDay;

        public TimeSpan DayToDayTime
        {
            get => _dayToDayTime;
            set => SetProperty(ref _dayToDayTime, value);
        }

        public DateTime DayToDayDateTime
        {
            get => _dayToDayDateTime;
            set => SetProperty(ref _dayToDayDateTime, value);
        }
    }
}