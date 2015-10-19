using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Messages;

namespace InkySigma.Authentication.ServiceProviders.EmailProvider
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage message);
    }
}