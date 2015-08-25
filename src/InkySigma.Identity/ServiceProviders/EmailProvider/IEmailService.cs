using System.Threading.Tasks;

namespace InkySigma.Identity.EmailProvider
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
        Task<bool> SendEmail(string subject, string body);
    }
}
