using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models.SugarBeatData
{
    public abstract class SugarBeatData : ModelBase
    {

        public SugarBeatData()
        {
        }

        public SugarBeatData(int Id, DateTime Created, int AccountId, string TransmitterId, double? BatteryVoltage, string FirmwareVersion) : base(Id)
        {
            this.Created = Created;
            this.AccountId = AccountId;
            this.TransmitterId = TransmitterId;
            this.BatteryVoltage = BatteryVoltage;
            this.FirmwareVersion = FirmwareVersion;
        }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public string TransmitterId { get; set; }

        public double? BatteryVoltage { get; set; }
        public string FirmwareVersion { get; set; }
    }
}