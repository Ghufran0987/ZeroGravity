using System.Collections.Generic;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class ApiCallResult<T>
    {
        private ApiCallResult(bool success, string errorMessage, T value)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Value = value;
        }

        private ApiCallResult(bool success, string errorMessage, ErrorReason reason)
        {
            Success = success;
            ErrorMessage = errorMessage;
            ErrorReason = reason;
        }

        public bool Success { get; }

        public string ErrorMessage { get; }
        public ErrorReason ErrorReason { get; }

        public T Value { get; set; }

        public static ApiCallResult<T> Ok(T value = default)
        {
            return new ApiCallResult<T>(true, string.Empty, value);
        }

        public static ApiCallResult<T> Error(string error, ErrorReason reason = ErrorReason.None)
        {
            return new ApiCallResult<T>(false, error, reason);
        }

        public static ApiCallResult<T> Error(string error, List<string> errorList,
            ErrorReason reason = ErrorReason.None)
        {
            var errorString = error + "\n";

            foreach (var item in errorList) errorString += item + "\n";

            return new ApiCallResult<T>(false, errorString, reason);
        }
    }
}