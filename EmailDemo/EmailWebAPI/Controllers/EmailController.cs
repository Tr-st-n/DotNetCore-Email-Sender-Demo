using EmailCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EmailCore.Results;
using EmailCore.Senders;

namespace EmailWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly EmailConfig _emConfig;
        private readonly ILogger<EmailController> _logger;

        public EmailController(ISender sender, EmailConfig config, ILogger<EmailController> logger)
        {
            _sender = sender;
            _emConfig = config;
            _logger = logger;
        }

        /// <summary> Sends an email. </summary>
        /// <param name="recipients"> Serialized JSON array containing an array of Addressee(s). </param>
        /// <param name="subject"> Subject of the email. </param>
        /// <param name="textbody"> Plain text body of the email. </param>
        /// <param name="FILES"> Files; one file per form-data pair, any UNIQUE key will be accepted. </param>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                // for our "lite web page" (DO NOT COPY PASTE THIS PLEASE)
                Response.Headers.Add("Access-Control-Allow-Origin", "*");

                var formCollection = Request.Form;

                var jString = formCollection["recipients"].ToString();
                var recipients = JsonConvert.DeserializeObject<List<Addressee>>(jString);

                if (recipients == null || recipients.Count == 0)
                    return StatusCode(400, "Recipients JSON was bad or contained no addressees.");

                foreach (Addressee recipient in recipients)
                {
                    if (string.IsNullOrEmpty(recipient.Address))
                        return StatusCode(400, "An email address value is null or empty.");
                    else if (await recipient.Valid() != true)
                        return StatusCode(400, $"Invalid email address: \"{recipient.Address}\".");
                }

                var subject = formCollection["subject"].ToString();
                var textbody = formCollection["textbody"].ToString();

                if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(textbody))
                    return StatusCode(400, $"An email subject OR text body is REQUIRED.");

                var files = formCollection.Files.Any() ? formCollection.Files : new FormFileCollection();

                return GetStatus(await _sender.SendAsync(new Message(recipients, subject, textbody, files)));
            }
            catch (Exception e)
            {
                _logger.LogError($"\n{e.Message}\n{e.StackTrace}");
                if (e is JsonException)
                {
                    return StatusCode(500, "An error occurred while deserializing \"recipients\" json string. Should look like: [{\"Name\":\"One Name Here\",\"Address\":\"user1@host.tld\"},{\"Name\":\"Another Here\",\"Address\":\"user2@host.tld\"}]");
                }
                return StatusCode(500, $"An error occurred while attempting to send email.");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, "Visit https://github.com/Tr-st-n for more info.");
        }

        /// <summary> Returns IActionResult using a EmailCore.BaseSenderResult. </summary>
        private IActionResult GetStatus(ISenderResult result)
        {
            if (result.Successful)
                return StatusCode(200);

            if (result is RichSenderResult richResult && richResult.HasErrors())
            {
                var lastError = richResult.LastError();

                if (lastError.Kind == SenderErrorKind.TimeOut)
                    return StatusCode((int)lastError.Kind, $"A timeout occurred while attempting to send an email (took >{_emConfig.TotalTimeout}ms).");
            }

            return StatusCode(500, $"An error occurred while attempting to send an email.");
        }
    }
}
