using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Messages;

namespace InkySigma.Authentication.ServiceProviders.EmailProvider
{
    public class EmailService : IEmailService
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public int Port { get; set; }

        public EmailService(string host, string userName, string password, string from, int port)
        {
            Host = host;
            UserName = userName;
            Password = password;
            From = from;
            Port = port;
        }

        public async Task<bool> SendEmail(EmailMessage message)
        {
            var client = new SmtpClient(Host, Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(UserName, Password)
            };
            await client.SendMailAsync(new MailMessage(From, message.Recipient)
            {
                Body = message.Body,
                AlternateViews =
                {
                    AlternateView.CreateAlternateViewFromString(message.Alternate, new ContentType("text/plain"))
                },
                Subject = message.Subject
            });
            return true;
        }
    }
}
