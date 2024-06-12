using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class PushNotificationTokenDataDto
    {
        public PushNotificationTokenDataDto()
        {
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Platform { get; set; }

        public string Token { get; set; }

        public DateTime LastUsed { get; set; }
    }
}