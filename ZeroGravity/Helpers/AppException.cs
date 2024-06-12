using System;
using System.Globalization;

namespace ZeroGravity.Helpers
{
    // custom exception class for throwing application specific exceptions 
    // that can be caught and handled within the application
    public class AppException : Exception
    {
        public AppException() : base() {}

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }

    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }
    }

    public class AccountNotVerifiedException : Exception
    {
        public AccountNotVerifiedException(string message) : base(message) { }
    }

    public class EmailOrPasswordIncorrectException : Exception
    {
        public EmailOrPasswordIncorrectException(string message) : base(message) { }
    }
}