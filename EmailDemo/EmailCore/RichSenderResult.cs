using System;
using System.Collections.Generic;

namespace EmailCore
{
    public class RichSenderResult : BaseSenderResult
    {
        public List<Exception> Exceptions { get; set; }
    }
}
