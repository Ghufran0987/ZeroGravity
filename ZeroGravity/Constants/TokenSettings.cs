using System;

namespace ZeroGravity.Constants
{
    public class TokenSettings
    {
        // JWT for each authorization required request
        public static DateTime JsonWebTokenExpiresIn => DateTime.UtcNow.AddDays(1);

        // Refresh token to get a new JWT, when JWT expired
        public static DateTime RefreshTokenExpiresIn => DateTime.UtcNow.AddDays(7);
    }
}
