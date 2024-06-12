using System;

namespace ZeroGravity.Shared.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
