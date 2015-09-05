using System.Threading.Tasks;

namespace InkySigma.Identity.ServiceProviders.EmailProvider
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
        Task<bool> SendEmail(string subject, string body);
    }
}
