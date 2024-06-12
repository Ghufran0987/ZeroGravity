using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Db.Models
{
    public class GlucoseData : ModelBase
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Glucose { get; set; }

        
        public List<SugarBeatDataBase> SugarBeatData { get; set; }

        public GlucoseData()
        {
            SugarBeatData = new List<SugarBeatDataBase>();
        }
   
    }
}
