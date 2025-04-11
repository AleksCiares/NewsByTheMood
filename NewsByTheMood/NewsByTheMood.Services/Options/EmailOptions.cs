
namespace NewsByTheMood.Services.Options
{
    public class EmailOptions
    {
        public static string Position => "EmailSender";
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public bool EnableSsl { get; set; }
    }
}
