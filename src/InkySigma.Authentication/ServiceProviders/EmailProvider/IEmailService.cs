using System.Threading.Tasks;

namespace InkySigma.Authentication.ServiceProviders.EmailProvider
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
    }
}