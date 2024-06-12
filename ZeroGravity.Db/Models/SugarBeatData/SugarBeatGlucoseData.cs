using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    [Index(nameof(AccountId), nameof(Created), nameof(TransmitterId), IsUnique = true)]
    public class SugarBeatGlucoseData : SugarBeatData
    {
        public double SensorValue { get; set; }
        public double? CE { get; set; }
        public double? RE { get; set; }
        public double? GlucoseValue { get; set; }
        public bool IsSessionFirstValue { get; set; }
        public bool IsSensorWarmedUp { get; set; }

        public int? SessionId { get; set; }
        public virtual SugarBeatSessionData Session { get; set; }

        public double? Temperature { get; set; }
    }
}