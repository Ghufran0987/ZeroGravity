using System;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    public class SugarBeatSettingData : SugarBeatData
    {
        public Guid DeviceId { get; set; }
        public DateTime? LastSyncedTime { get; set; }
    }
}