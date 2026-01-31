using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Api.Settings;

namespace SurveyBasket.Api.Services
{
    public class EmailService(IOptions<MailSettings> options , ILogger<EmailService> logger) : IEmailSender
    {
        private readonly MailSettings _mailSettings = options.Value;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(email));
            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            _logger.LogInformation("Send email message to {email} with subject {subject}", email, subject);
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}
