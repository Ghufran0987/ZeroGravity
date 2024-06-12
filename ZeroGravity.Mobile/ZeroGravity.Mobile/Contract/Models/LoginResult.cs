using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class LoginResult : ResultBase
    {
        private LoginResult(bool success, string errorMessage, ErrorReason reason) : base(success, errorMessage, reason)
        {
        }

        public static LoginResult Ok()
        {
            return new LoginResult(true, string.Empty, ErrorReason.None);
        }

        public static LoginResult Error(string error, ErrorReason reason)
        {
            return new LoginResult(false, error, reason);
        }
    }
}
