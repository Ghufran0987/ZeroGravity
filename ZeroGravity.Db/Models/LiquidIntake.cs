using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId), nameof(Created))]
    public class LiquidIntake : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public LiquidType LiquidType { get; set; }

        public string Name { get; set; }

        public double AmountMl { get; set; }

        public DateTime Created { get; set; }
    }
}