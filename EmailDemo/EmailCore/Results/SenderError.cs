namespace EmailCore.Results
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
}