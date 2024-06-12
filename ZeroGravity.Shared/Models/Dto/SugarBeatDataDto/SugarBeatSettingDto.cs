using System;

namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatSettingDto : SugarBeatDto
    {
        public Guid DeviceId { get; set; }
        public DateTime? LastSyncedTime { get; set; }
    }
}