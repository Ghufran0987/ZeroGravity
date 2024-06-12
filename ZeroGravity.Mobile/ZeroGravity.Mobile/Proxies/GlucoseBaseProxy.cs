using System;
using System.Collections.Generic;
using Prism.Mvvm;
using ZeroGravity.Mobile.Proxies.SugarBeatDataProxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class GlucoseBaseProxy : BindableBase
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get => _dateTime;
            set => SetProperty(ref _dateTime, value);
        }

        private double _glucose;
        public double Glucose
        {
            get => _glucose;
            set => SetProperty(ref _glucose, value);
        }
    }
}
