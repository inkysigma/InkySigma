using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using InkySigma.Authentication.ServiceProviders.EmailProvider;

namespace InkySigma.Authentication.Dapper.Services
{
    public class EmailService : IEmailService
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }

        public EmailService(string host, string userName, string password, string from)
        {
            Host = host;
            UserName = userName;
            Password = password;
            From = from;
        }

        public async Task<bool> SendEmail(EmailMessage message)
        {
            var client = new SmtpClient(Host);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(UserName, Password);
            client.SendMailAsync(From, message.)
            return true;
        }
    }
}
