using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    public class SugarBeatAlertData : SugarBeatData
    {
        public AlertCode Code { get; set; }
        public CRCCodes CriticalCode { get; set; }
        public double? Temperature { get; set; }
    }
}