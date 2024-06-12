using System;
using System.Collections.Generic;
using System.Text;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies.SugarBeatDataProxy
{
    public class SugarBeatBaseProxy : ProxyBase
    {
        public int GlucoseDataId { get; set; }
        public double? Amount { get; set; }
    }
}
