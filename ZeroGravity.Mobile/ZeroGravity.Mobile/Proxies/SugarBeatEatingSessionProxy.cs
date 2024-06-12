using System;
using Prism.Mvvm;

namespace ZeroGravity.Mobile.Proxies
{
    public class SugarBeatEatingSessionProxy : BindableBase
    {

        public int Id { get; set; }
        public int AccountId { get; set; }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }



        private decimal _metabolicScore;
        public decimal MetabolicScore
        {
            get => _metabolicScore;
            set => SetProperty(ref _metabolicScore, (int)Math.Floor(value));
        }


        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

    }

}
