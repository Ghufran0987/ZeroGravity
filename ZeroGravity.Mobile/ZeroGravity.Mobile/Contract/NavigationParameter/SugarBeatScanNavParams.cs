using System;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class SugarBeatScanNavParams
    {
        public SugarBeatScanNavParams(string deviceAddress, DateTime dateTime)
        {
            DeviceAddress = deviceAddress;
            DateTime = dateTime;
        }

        public string DeviceAddress { get; }
        public DateTime DateTime { get; }
    }
}