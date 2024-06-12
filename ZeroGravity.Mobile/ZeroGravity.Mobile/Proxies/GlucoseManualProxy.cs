using System;

namespace ZeroGravity.Mobile.Proxies
{
    public class GlucoseManualProxy : GlucoseBaseProxy
    {
        private TimeSpan _measurementTime;

        public TimeSpan MeasurementTime
        {
            get => _measurementTime;
            set => SetProperty(ref _measurementTime, value);
        }
    }
}
