using System;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies.SugarBeatDataProxy
{
    public class SugarBeatProxy : ProxyBase
    {
        public DateTime Created { get; set; }
        public int AccountId { get; set; }
        public string TransmitterId { get; set; }
        public double? BatteryVoltage { get; set; }
        public string FirmwareVersion { get; set; }
    }
}
