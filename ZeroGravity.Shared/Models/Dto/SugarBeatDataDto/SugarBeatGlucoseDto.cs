namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatGlucoseDto : SugarBeatDto
    {
        public double SensorValue { get; set; }
        public double? CE { get; set; }
        public double? RE { get; set; }
        public double? GlucoseValue { get; set; }
        public bool IsSessionFirstValue { get; set; }
        public bool IsSensorWarmedUp { get; set; }
        public int? SessionId { get; set; }
        public double? Temperature { get; set; }
    }
}