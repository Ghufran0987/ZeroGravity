using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class FastingData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Created { get; set; }

    }
}