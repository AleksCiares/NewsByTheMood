using Microsoft.AspNetCore.Identity.UI.Services;

namespace NewsByTheMood.Services.EmailProvider
{
    public class EmptyEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Task.CompletedTask;
        }
    }
}
