using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models.Users
{
    [Index(nameof(AccountId), nameof(Platform), nameof(Model), nameof(OSVersion), nameof(AppVersion), IsUnique = true)]
    public class AppInfoData : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        public string Platform { get; set; }

        public string Model { get; set; }

        public string OSVersion { get; set; }

        public string AppVersion { get; set; }

        public DateTime LastAccessed { get; set; }

        public string Locale { get; set; }

        public string Timezone { get; set; }

    }
}