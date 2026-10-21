using System;
using System.Net;
using log4net;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Castle.Core;
using DaysOn.Common.Events.Events;
using DaysOn.Common.MessageHandlers;
using DaysOn.Common.Utilities.Extension;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DaysOn.Functions.Api.BusEventHandlers
{
    [CastleComponent(typeof(IMessageHandlerAsync<SendEmailEvent>))]
    public class SendEmailEventHandler : IMessageHandlerAsync<SendEmailEvent>
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<bool> Handle(SendEmailEvent message)
        {
            _logger.LogInf($"SendEmailEventHandler called with message {JsonConvertExt.SerializeObject(message)}");
            var apiKey = "REPLACE_WITH_YOUR_VALUE";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("user@example.invalid", "Support Team"),
                Subject = message.Subject,
                PlainTextContent = message.PlainTextContent,
                HtmlContent = message.BodyHtml
            };

            if (message.EmailTo.Contains("muva"))
            {
                msg.AddTo(new EmailAddress("user@example.invalid"));
                msg.AddBcc(new EmailAddress("user@example.invalid"));
            }
            else
            {
                _logger.LogInf($"SendEmailEventHandler completed skipped {message.EmailTo} it does not contains muva");
                return true;
            }

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogErr($"SendEmailEventHandler fail to send email to {message.EmailTo} {JsonConvertExt.SerializeObject(response)}");
                return false;
            }
            _logger.LogInf($"SendEmailEventHandler completed send email to {message.EmailTo} {JsonConvertExt.SerializeObject(response)}");
            return true;
        }

        public void LogError(Exception ex)
        {
            _logger.LogInf($"SendEmailEventHandler throws exception {JsonConvertExt.SerializeObject(ex)}");
        }

        public void LogMessage(string message)
        {
            _logger.LogInf(message);
        }
    }
}
