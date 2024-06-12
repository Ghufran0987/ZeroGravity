using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatAlertDto : SugarBeatDto
    {
        public AlertCode Code { get; set; }
        public CRCCodes CriticalCode { get; set; }
        public double? Temperature { get; set; }
    }
}