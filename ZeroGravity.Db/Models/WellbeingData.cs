using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId), nameof(Created))]
    public class WellbeingData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public WellbeingType Rating { get; set; }

        public DateTime Created { get; set; }
    }
}