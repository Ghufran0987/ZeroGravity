using System;
using System.Collections.Generic;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Shared.Models.Dto
{
    public class GlucoseDataDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public DateTime Date { get; set; }
        public double Glucose { get; set; }

        public List<SugarBeatDataBaseDto> SugarBeatData { get; set; }
    }
}
