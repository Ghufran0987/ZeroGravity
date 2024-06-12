using System;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class BleScanResult : ResultBase
    {
        private BleScanResult(bool success, bool scanning, Exception exception, string message,
            SugarBeatDevice device)
            : base(success, message, ErrorReason.BleScan)
        {
            Scanning = scanning;
            Exception = exception;
            Device = device;
        }

        public BleScanResult(Exception exception) : this(false, false, exception, exception.Message, default)
        {
            Exception = exception;
        }

        public BleScanResult(SugarBeatDevice device) : this(true, false, null, string.Empty, device)
        {
        }

        public static BleScanResult IsScanning => new BleScanResult(false, true, null, string.Empty, default);

        public Exception Exception { get; }
        public bool Scanning { get; }
        public SugarBeatDevice Device { get; }
    }
}