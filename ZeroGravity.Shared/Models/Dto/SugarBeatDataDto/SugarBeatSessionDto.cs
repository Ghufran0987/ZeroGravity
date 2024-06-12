using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto.SugarBeatDataDto
{
    public class SugarBeatSessionDto : SugarBeatDto
    {
        public DateTime? EndTime { get; set; }

        public int StartAlertId { get; set; }

        public virtual SugarBeatAlertDto StartAlert { get; set; }

        public int? EndAlertId { get; set; }
        public virtual SugarBeatAlertDto EndAlert { get; set; }

        public ICollection<SugarBeatGlucoseDto> GlucoseDatas { get; set; }
    }
}