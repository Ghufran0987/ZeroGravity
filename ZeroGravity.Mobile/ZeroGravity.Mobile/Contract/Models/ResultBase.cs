using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class ResultBase
    {
        public bool Success { get; }
        public string Message { get; }
        public ErrorReason ErrorReason { get;}

        protected ResultBase(bool success, string message, ErrorReason reason)
        {
            Success = success;
            Message = message;
            ErrorReason = reason;
        }
    }
}
