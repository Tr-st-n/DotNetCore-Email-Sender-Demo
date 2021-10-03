using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace EmailCore
{
    public class Sender : ISender
    {
        private CancellationTokenSource _cts = null;
        private readonly EmailConfig _config;
        private readonly ILogger<Sender> _logger;
        public Sender(EmailConfig emailConfig, ILogger<Sender> logger)
        {
            _config = emailConfig;
            _logger = logger;
        }

        /// <summary> Asynchronously sends an email using a EmailCore.Message argument. Returns a EmailCore.BaseSenderResult. </summary>
        public async Task<BaseSenderResult> SendAsync(Message message)
        {
            var builtMessage = BuildMessage(message);
            return await TrySendAsync(builtMessage);
        }

        /// <summary> Builds and returns a MimeKit.MimeMessage using a EmailCore.Message argument. </summary>
        private MimeMessage BuildMessage(Message message)
        {
            var builtMessage = new MimeMessage();
            builtMessage.From.Add(new MailboxAddress(_config.SentFromName, _config.SentFromAddress));
            builtMessage.To.AddRange(message.To);
            builtMessage.Subject = message.Subject;
            var builder = new BodyBuilder
            {
                TextBody = message.Body
            };
            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                byte[] fileByteArray;
                foreach (var attatchment in message.Attachments)
                {
                    using (var stream = new MemoryStream())
                    {
                        attatchment.CopyTo(stream);
                        fileByteArray = stream.ToArray();
                    }
                    builder.Attachments.Add(attatchment.FileName, fileByteArray, ContentType.Parse(attatchment.ContentType));
                }
            }
            builtMessage.Body = builder.ToMessageBody();

            return builtMessage;
        }

        /// <summary> Attempts to send an email X times with Y interval between attempts and a timeout per attempt of Z (set in appsettings.json) using a MimeKit.MimeMessage argument. Returns a EmailCore.RichSenderResult. </summary>
        private async Task<RichSenderResult> TrySendAsync(MimeMessage mimeMessage)
        {
            var result = new RichSenderResult 
            { 
                Exceptions = new List<Exception>()
            };
            int attempts = 0;
            Exception error;
            do
            {
                error = null;
                using (var client = new SmtpClient())
                {
                    try
                    {
                        _cts = new CancellationTokenSource();
                        _cts.CancelAfter(_config.TotalTimeout);
                        await client.ConnectAsync(_config.SMTPHost, _config.Port, true, _cts.Token);
                        await client.AuthenticateAsync(_config.Username, _config.Password, _cts.Token);
                        await client.SendAsync(mimeMessage, _cts.Token);
                    }
                    catch (Exception e)
                    {
                        error = e;
                    }
                    finally
                    {
                        await client.DisconnectAsync(true);
                        client.Dispose();
                        _cts.Dispose();
                    }
                }

                if (error != null)
                {
                    result.Exceptions.Add(error);

                    if (_config.FailDelay > 0)
                        await Task.Delay(_config.FailDelay);

                    attempts++;
                }
            } while (error != null & attempts < _config.SendAttempts);
            result.Successful = error == null;
            LogResult(result.Successful, attempts, mimeMessage, result.Exceptions);
            return result;
        }

        /// <summary> Logs the results of a set of send attempts. </summary>
        /// <param name="success"> Whether or not the send succeeded. </param>
        /// <param name="attempts"> How many attempts it took to succeed (successful attempts included). </param>
        /// <param name="message"> The MimeKit.MimeMessage that was sent. </param>
        /// <param name="exceptions"> Any exceptions that occurred while trying to send the message. </param>
        private void LogResult(bool success, int attempts, MimeMessage message, List<Exception> exceptions)
        {
            string recipientInfo = BuildRecipientInfo(message) ?? "None.";
            string lead = success ? "Successfully sent" : "Failed to send";
            string exceptionInfo = BuildExceptionInfo(exceptions);
            int attCount = message.Attachments.Count(); // linq - i know
            _logger.LogInformation("{success} email to \"{recipients}\" after {attempts} attempt(s).\nSubject: \"{subject}\"\nText body: \"{textBody}\"\nNo. of Attachments: {noOfAttatchments}\nExceptions: {exceptions}", lead, recipientInfo, attempts, message.Subject, message.TextBody, attCount, exceptionInfo);
        }

        #region Logger Helpers
        /// <summary> Returns a string containing the MimeKit.MimeMessage (argument) recipient information. </summary>
        private static string BuildRecipientInfo(MimeMessage message)
        {
            var builder = new StringBuilder();
            foreach (MailboxAddress address in message.To.Mailboxes)
            {
                // name
                builder.Append('(');
                if (string.IsNullOrEmpty(address.Name))
                    builder.Append("NULL");
                else
                    builder.Append(address.Name);
                builder.Append(") ");

                // email address
                if (string.IsNullOrEmpty(address.Address))
                    builder.Append("NULL");
                else
                    builder.Append(address.Address);

                // comma
                builder.Append(", ");
            }
            // remove trailing comma
            builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }

        /// <summary> Returns a string containing the Exception messages and stack traces of a List<Exception>. Returns null if List<Exception> is empty or null. </summary>
        private static string BuildExceptionInfo(List<Exception> exceptions)
        {
            if (exceptions == null || exceptions.Count == 0)
                return null;

            var builder = new StringBuilder();

            for (int i = 0; i < exceptions.Count; i++)
            {
                // break line
                builder.Append("\n");
                // ex message
                builder.Append(i + 1);
                builder.Append(") ");
                builder.Append(exceptions[i].Message);
                builder.Append(' ');
                // ex stack trace
                builder.Append(exceptions[i].StackTrace);
                builder.Append(", ");
            }
            // remove trailing comma
            builder.Remove(builder.Length - 2, 2);

            return builder.ToString();
        }
        #endregion
    }
}
