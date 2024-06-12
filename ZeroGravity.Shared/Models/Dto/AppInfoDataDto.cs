using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class AppInfoDataDto
    {
        public AppInfoDataDto()
        {
        }

        public int Id { get; set; }

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