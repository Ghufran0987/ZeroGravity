using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class PasswordForgotResult : ResultBase
    {
        private PasswordForgotResult(bool success, string msg, ErrorReason reason) : base (success, msg, reason)
        {
        }

        public static PasswordForgotResult Ok(string msg = "")
        {
            return new PasswordForgotResult(true, msg , ErrorReason.None);
        }

        public static PasswordForgotResult Error(string errorMsg, ErrorReason reason)
        {
            return new PasswordForgotResult(false, errorMsg, reason);
        }
    }
}
