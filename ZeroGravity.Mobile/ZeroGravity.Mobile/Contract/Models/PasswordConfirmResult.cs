using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class PasswordConfirmResult : ResultBase
    {
        private PasswordConfirmResult(bool success, string msg, ErrorReason reason) : base(success, msg, reason)
        {
        }

        public static PasswordConfirmResult Ok(string msg)
        {
            return new PasswordConfirmResult(true, msg, ErrorReason.None);
        }

        public static PasswordConfirmResult Error(string errorMsg, ErrorReason reason)
        {
            return new PasswordConfirmResult(false, errorMsg, reason);
        }
    }
}
