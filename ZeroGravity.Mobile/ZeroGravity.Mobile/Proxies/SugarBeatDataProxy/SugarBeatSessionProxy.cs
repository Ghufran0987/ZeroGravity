using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Mobile.Proxies.SugarBeatDataProxy
{
    public class SugarBeatSessionProxy : SugarBeatProxy
    {

        public DateTime? EndTime { get; set; }

        public int StartAlertId { get; set; }

        public virtual SugarBeatAlertProxy StartAlert { get; set; }

        public int? EndAlertId { get; set; }
        public virtual SugarBeatAlertProxy EndAlert { get; set; }

        public ICollection<SugarBeatGlucoseProxy> GlucoseDatas { get; set; }

    }
}
