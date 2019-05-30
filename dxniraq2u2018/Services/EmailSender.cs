using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using SendGrid;

namespace dxniraq2u2018.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("info@dxniraq2u.com", "DXNiraq2u"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            string Adminemail = "info@filspay.com";
            msg.AddTo(new EmailAddress(email));
            msg.AddBcc(new EmailAddress(Adminemail));
            return client.SendEmailAsync(msg);
        }
    }
}
