using System.Collections.Generic;

namespace ZeroGravity.Mobile.Contract.Exceptions.Messages
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
