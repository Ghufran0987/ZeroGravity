using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public DateTimeDisplayType DateTimeDisplayType { get; set; }
        public UnitDisplayType UnitDisplayType { get; set; }
    }
}
