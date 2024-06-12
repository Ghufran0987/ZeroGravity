using System;
using System.Collections.Generic;
using System.Text;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Proxies.SugarBeatDataProxy
{
    public class SugarBeatAlertProxy : SugarBeatBaseProxy
    {
        public AlertCode Code { get; set; }
        public CRCCodes CriticalCode { get; set; }
    }
}
