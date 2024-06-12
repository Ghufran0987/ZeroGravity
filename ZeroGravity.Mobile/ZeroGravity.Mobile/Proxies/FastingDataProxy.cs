using System;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class FastingDataProxy : ProxyBase
    {
        public int AccountId { get; set; }


        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { SetProperty(ref _startDateTime, value); }
        }

        private DateTime _finishDateTime;
        public DateTime FinishDateTime
        {
            get => _finishDateTime;
            set => SetProperty(ref _finishDateTime, value);
        }

        private double _duration;
        public double Duration
        {
            get => _duration;
            set
            {
                SetProperty(ref _duration, value);

                //CalculateFinishTime();
            }
        }

        private DateTime _created;
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }




        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                SetProperty(ref _startTime, value);

                SetStartDateTime();
                //CalculateFinishTime();
            }
        }









        private void CalculateFinishTime()
        {
            var tempDate = new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
            var finishTIme = tempDate.AddHours(Duration);
            FinishDateTime = new DateTime(finishTIme.Year, finishTIme.Month, finishTIme.Day, finishTIme.Hour, finishTIme.Minute, finishTIme.Second);
        }

        private void SetStartDateTime()
        {
            StartDateTime = new DateTime(Created.Year, Created.Month, Created.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
        }
    }
}