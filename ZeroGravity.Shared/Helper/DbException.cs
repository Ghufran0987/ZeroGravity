using System;
using System.Globalization;

namespace ZeroGravity.Shared.Helper
{
    public class DbException : Exception
    {
        public DbException()
        {
        }

        public DbException(string message) : base(message)
        {
        }

        public DbException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}