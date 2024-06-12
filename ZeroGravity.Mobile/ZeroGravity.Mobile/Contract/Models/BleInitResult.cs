using System;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class BleInitResult : ResultBase
    {
        private BleInitResult() : base(true, string.Empty, ErrorReason.BleInit)
        {
        }

        private BleInitResult(bool success) : base(success, string.Empty, ErrorReason.BleInit)
        {
        }

        public BleInitResult(Exception exception) : base(false, exception.Message, ErrorReason.BleInit)
        {
            Exception = exception;
        }

        public Exception Exception { get; }

        public static BleInitResult Successful => new BleInitResult(); 
        public static BleInitResult Unsuccessful => new BleInitResult(false);
    }
}