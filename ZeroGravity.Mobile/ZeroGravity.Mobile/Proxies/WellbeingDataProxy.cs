using System;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class WellbeingDataProxy : ProxyBase
    {

        public int AccountId { get; set; }

        public int Rating { get; set; }

        public DateTime Created { get; set; }
    }
}