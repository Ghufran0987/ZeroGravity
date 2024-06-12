using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ZeroGravity.Db.Models.Users
{
    [Index(nameof(Email), IsUnique = true)]
    public class ConfirmedSensorPurchaseUserData: ModelBase
    {
        [Required]
        public string Email { get; set; }
        public string OnboardingAccessToken { get; set; }
        public DateTime? OnboardingAccessDateTime { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
    }
}