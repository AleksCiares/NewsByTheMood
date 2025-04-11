using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsByTheMood.Services.Options;

namespace NewsByTheMood.Services.EmailProvider
{
    public class PrettyEmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<PrettyEmailSender> _logger;

        public PrettyEmailSender(IOptions<EmailOptions> emailOptions, ILogger<PrettyEmailSender> logger)
        {
            _emailOptions = emailOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using var client = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port)
                {
                    Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
                    EnableSsl = _emailOptions.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.Headers.Add("X-Mailer", "NewsByTheMood");
                mailMessage.Headers.Add("X-Priority", "3");
                mailMessage.ReplyToList.Add(new MailAddress(_emailOptions.FromEmail));

                mailMessage.To.Add(email);

                _logger.LogInformation("Sending email to {Email} with subject {Subject}", email, subject);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {email}.");
            }
        }
    }
}

