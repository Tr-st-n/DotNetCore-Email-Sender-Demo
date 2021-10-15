using System.Collections.Generic;

namespace EmailCore.Results
{
    public class RichSenderResult : BaseSenderResult
    {
        public RichSenderResult()
        {
            Errors = new List<SenderError>();
        }
        public List<SenderError> Errors { get; set; }

        public bool HasErrors() => Errors != null && Errors.Count > 0;
        public SenderError LastError()
        {
            if (!HasErrors())
                return null;

            return Errors[^1];
        }
    }
}
