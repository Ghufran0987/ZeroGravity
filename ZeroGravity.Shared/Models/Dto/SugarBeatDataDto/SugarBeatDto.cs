using System;

namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public int AccountId { get; set; }

        public string TransmitterId { get; set; }

        public double? BatteryVoltage { get; set; }
        public string FirmwareVersion { get; set; }
    }
}