using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class FastingSetting : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public bool SkipBreakfast { get; set; }

        public bool SkipLunch { get; set; }

        public bool SkipDinner { get; set; }

        public bool IncludeMondays { get; set; }

        public bool IncludeTuesdays { get; set; }

        public bool IncludeWednesdays { get; set; }

        public bool IncludeThursdays { get; set; }

        public bool IncludeFridays { get; set; }

        public bool Includesaturdays { get; set; }

        public bool IncludeSundays { get; set; }
    }
}