using Prism.Mvvm;
using System;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies
{
    public class WeightItemProxy : BindableBase
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int WeightTrackerId { get; set; }

        private double _value;

        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private DateTime _created;

        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(ref _created, value); }
        }

        public string GetCreated
        {
            get { return string.Format("{0:dddd dd'th' MMM}", Created); }
        }

        public string GetValue
        {
            get
            {
                return string.Format("{0:0.0} ", Value) + DispalyUnit();
            }
        }

        public string GetUnit
        {
            get { return DispalyUnit(); }
        }

        private string DispalyUnit()
        {
            var displayPrefences = DisplayConversionService.GetDisplayPrefences();
            var disPlayUnit = string.Empty;
            switch (displayPrefences.UnitDisplayType)
            {
                case Shared.Enums.UnitDisplayType.Imperial:
                    disPlayUnit = "Lbs";
                    //   return string.Format("{0} {1}", DisplayConversionService.ConvertWeight(Value, UnitDisplayType.Imperial), disPlayUnit);
                    break;

                case Shared.Enums.UnitDisplayType.Metric:
                    disPlayUnit = "Kg";
                    //   return string.Format("{0} {1}", Value, disPlayUnit);
                    break;

                default:
                    disPlayUnit = "Kg";
                    //   return string.Format("{0} {1}", Value, disPlayUnit);
                    break;
            }
            return disPlayUnit;
        }
    }
}