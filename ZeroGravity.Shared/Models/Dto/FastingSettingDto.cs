using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Shared.Models.Dto
{
    public class FastingSettingDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool SkipBreakfast { get; set; }

        public bool SkipLunch { get; set; }

        public bool SkipDinner { get; set; }

        public bool IncludeMondays { get; set; }

        public bool IncludeTuesdays { get; set; }

        public bool IncludeWednesdays { get; set; }

        public bool IncludeThursdays { get; set; }

        public bool IncludeFridays { get; set; }

        public bool IncludeSaturdays { get; set; }

        public bool IncludeSundays { get; set; }
    }
}
