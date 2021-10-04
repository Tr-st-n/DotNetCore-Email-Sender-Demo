using System;
using System.Collections.Generic;
using System.Text;

namespace EmailCore
{
    public class SenderError
    {
        public SenderError(SenderErrorKind kind, string message)
        {
            Kind = kind;
            Message = message;
        }
        public SenderErrorKind Kind {  get; set; }
        public string Message {  get; set; }
    }

    public enum SenderErrorKind : int 
    { 
        Generic = 500, // Internal Server Error
        TimeOut = 408 // Request Timeout
    }
}
