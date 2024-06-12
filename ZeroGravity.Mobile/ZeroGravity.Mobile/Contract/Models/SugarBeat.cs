using System;

namespace ZeroGravity.Mobile.Contract.Models
{
    public abstract class SugarBeat
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int AccountId { get; set; }
        
        public string TransmitterId { get; set; }

        public double? BatteryVoltage { get; set; }

        public string FirmwareVersion { get; set; }
    }
}

