using System;

namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatEatingSessionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal MetabolicScore { get; set; }
        public bool IsCompleted { get; set; }
    }
}