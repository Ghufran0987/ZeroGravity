using System.Collections.Generic;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class SugarBeatSession : SugarBeat
    {
        public SugarBeatSession()
        {
        }

        public double? EndTime { get; set; }
       
        public int StartAlertId { get; set; }

        public virtual SugarBeatAlert StartAlert { get; set; }

        public int? EndAlertId { get; set; }

        public virtual SugarBeatAlert EndAlert { get; set; }

        public ICollection<SugarBeatGlucose> GlucoseData { get; set; }
    }
}

