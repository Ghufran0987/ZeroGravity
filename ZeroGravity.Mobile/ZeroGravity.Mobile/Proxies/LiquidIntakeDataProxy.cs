using System;
using Prism.Mvvm;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class LiquidIntakeDataProxy : BindableBase
    {
        private double _amountMl;

        private DateTime _createDateTime;

        private string _name;

        private TimeSpan _time;

        public LiquidIntakeDataProxy(LiquidType type)
        {
            LiquidType = type;
            AmountMl = 250;
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public LiquidType LiquidType { get; }

        public double AmountMl
        {
            get => _amountMl;
            set => SetProperty(ref _amountMl, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DateTime CreateDateTime
        {
            get => _createDateTime;
            set => SetProperty(ref _createDateTime, value);
        }

        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private string _timeDisplayFormat;

        public string TimeDisplayFormat
        {
            get => _timeDisplayFormat;
            set => SetProperty(ref _timeDisplayFormat, value);
        }
    }
}