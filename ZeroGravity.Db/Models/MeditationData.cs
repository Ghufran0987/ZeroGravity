using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class MeditationData : ModelBase
    {
        [Required] 
        public int AccountId { get; set; }

        public DateTime Created { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
