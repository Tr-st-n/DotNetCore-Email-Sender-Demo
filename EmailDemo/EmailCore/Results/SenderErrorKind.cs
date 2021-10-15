namespace EmailCore.Results
{
    public enum SenderErrorKind : int
    {
        Generic = 500, // Internal Server Error
        TimeOut = 408 // Request Timeout
    }
}