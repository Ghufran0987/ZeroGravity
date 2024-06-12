using System;
using Prism.Mvvm;

namespace ZeroGravity.Mobile.Proxies
{
    public class MeditationDataProxy : BindableBase
    {
        public int Id { get; set; }

        public int AccountId { get; set; }


        private DateTime _createDateTime;
        public DateTime CreateDateTime
        {
            get => _createDateTime;
            set => SetProperty(ref _createDateTime, value);
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }
    }
}
