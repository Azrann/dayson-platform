using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.Events.Events;
using DaysOn.Functions.Api.BusEventHandlers;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DaysOn.Functions.Test.Integration
{
    [TestFixture]
    public class TestEmailService
    {
        [Test]
        public async void WhenISendEmail_IGetNoExceptions()
        {
            var apiKey = "REPLACE_WITH_YOUR_VALUE";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("test@example.com", "DX Team"),
                Subject = "Ey los email funcionan!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("user@example.invalid", "Tupas"));
            msg.AddTo(new EmailAddress("user@example.invalid", "Alberto"));

            var response = await client.SendEmailAsync(msg);
        }

        [Test]
        public async void WhenICallTheHandlerIReceiveAnEmail()
        {
            SendEmailEventHandler handler = new SendEmailEventHandler();
            await handler.Handle(new SendEmailEvent
            {
                EmailTo = "user@example.invalid",
                Subject = "Your account is ready.",
                BodyHtml = @"You have been created an account at HRPlan, set up your account in the following link <a href=""http://www.dayson-ui.gearhost.com/password/be2a0b49-fa1c-4ad7-a39a-17ef2f1a3abb"">click here</a>"
            });
        }
    }
}
