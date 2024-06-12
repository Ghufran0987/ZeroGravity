using System;

namespace ZeroGravity.Shared.Exceptions
{
    public class ZeroGravityNotSupportedException : Exception
    {
        public ZeroGravityNotSupportedException()
        {
        }

        public ZeroGravityNotSupportedException(string message) : base(message)
        {
        }

        public ZeroGravityNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
