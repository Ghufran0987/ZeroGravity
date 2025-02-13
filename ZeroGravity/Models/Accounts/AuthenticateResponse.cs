using System;
using System.Text.Json.Serialization;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Models.Accounts
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public DateTimeDisplayType DateTimeDisplayType { get; set; }
        public UnitDisplayType UnitDisplayType { get; set; }
    }
}