namespace EmailCore
{
    /// <summary> Set in appsettings.json </summary>
    public class EmailConfig
    {
        // Name to display in email.
        public string SentFromName { get; set; }

        // Address to send from.
        public string SentFromAddress { get; set; }

        // SMTP host.
        public string SMTPHost { get; set; }

        // SMTP host Port.
        public int Port { get; set; }

        // Attempts to make in sending (max).
        public int SendAttempts { get; set; }

        // Time to wait in milliseconds if send failed.
        public int FailDelay { get; set; }

        // Time to wait in milliseconds before canceling task.
        public int TotalTimeout { get; set; }

        // Email username.
        public string Username { get; set; }

        // Email password.
        public string Password { get; set; }
    }
}
