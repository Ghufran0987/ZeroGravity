using Newtonsoft.Json;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class RegisterResult : ResultBase
    {
        private RegisterResult(bool success, string msg, ErrorReason reason) : base(success, msg, reason)
        {
        }

        public static RegisterResult Ok(string msg)
        {
            return new RegisterResult(true, msg, ErrorReason.None);
        }

        public static RegisterResult Error(string errorMsg, ErrorReason reason)
        {
            return new RegisterResult(false, errorMsg, reason);
        }
    }
}
