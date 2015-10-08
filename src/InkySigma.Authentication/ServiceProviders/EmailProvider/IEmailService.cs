using System.Threading.Tasks;

namespace InkySigma.Authentication.ServiceProviders.EmailProvider
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
        Task<bool> SendEmail(string subject, string body);
    }
}
