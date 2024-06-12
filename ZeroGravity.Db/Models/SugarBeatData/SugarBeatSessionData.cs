using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    [Index(nameof(AccountId))]
    public class SugarBeatSessionData : SugarBeatData
    {
        public SugarBeatSessionData()
        {
        }

        public SugarBeatSessionData(int Id, DateTime Created, int AccountId, string TransmitterId, double? BatteryVoltage, string FirmwareVersion, DateTime? EndTime, int StartAlertId, SugarBeatAlertData StartAlert, int? EndAlertId, SugarBeatAlertData EndAlert, ICollection<SugarBeatGlucoseData> GlucoseDatas) : base(Id, Created, AccountId, TransmitterId, BatteryVoltage, FirmwareVersion)
        {
            this.EndTime = EndTime;
            this.StartAlertId = StartAlertId;
            this.StartAlert = StartAlert;
            this.EndAlertId = EndAlertId;
            this.EndAlert = EndAlert;
            this.GlucoseDatas = GlucoseDatas;
        }

        public DateTime? EndTime { get; set; }

        [Required]
        public int StartAlertId { get; set; }

        public virtual SugarBeatAlertData StartAlert { get; set; }

        public int? EndAlertId { get; set; }
        public virtual SugarBeatAlertData EndAlert { get; set; }

        public ICollection<SugarBeatGlucoseData> GlucoseDatas { get; set; }
    }
}