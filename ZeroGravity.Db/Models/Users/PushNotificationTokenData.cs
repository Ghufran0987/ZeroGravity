using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models.Users
{
    [Index(nameof(AccountId), nameof(Platform), nameof(Token), IsUnique = true)]
    public class PushNotificationTokenData : ModelBase
    {
        [Required] public int AccountId { get; set; }

        public string Platform { get; set; }

        public string Token { get; set; }

        public DateTime LastUsed { get; set; }
    }
}