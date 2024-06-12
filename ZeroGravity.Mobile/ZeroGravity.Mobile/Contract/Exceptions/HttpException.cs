using System;
using System.Collections.Generic;

namespace ZeroGravity.Mobile.Contract.Exceptions
{
    public class HttpException : Exception
    {
        public string CustomErrorMessage { get; private set; }

        public List<string> ErrorList { get; private set; }

        public HttpException()
        {
            
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, string customErrorMessage) : base(message)
        {
            CustomErrorMessage = customErrorMessage;
        }

        public HttpException(string message, string customErrorMessage, List<string> errorList) : base(message)
        {
            CustomErrorMessage = customErrorMessage;

            ErrorList = errorList;
        }
    }
}
