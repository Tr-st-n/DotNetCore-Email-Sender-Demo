using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;

namespace EmailCore
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public IFormFileCollection Attatchments { get; set; }

        /// <summary> Message object used by the ISender interface. </summary>
        /// <param name="recipients"> A List of Addressee(s) ...each containing an email address and (sometimes) a name. </param>
        /// <param name="subject"> The subject of the email. </param>
        /// <param name="body"> The plain text body of the email. </param>
        /// <param name="attatchments"> The attachments of the email. </param>
        public Message(List<Addressee> recipients, string subject, string body, IFormFileCollection attatchments)
        {
            To = new List<MailboxAddress>();
            foreach (Addressee recipient in recipients)
                To.Add(new MailboxAddress(recipient.Name, recipient.Address));
            Subject = subject;
            Body = body;
            Attatchments = attatchments;
        }
    }
}
