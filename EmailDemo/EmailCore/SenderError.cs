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

    public enum SenderErrorKind { Generic, TimeOut }
}
