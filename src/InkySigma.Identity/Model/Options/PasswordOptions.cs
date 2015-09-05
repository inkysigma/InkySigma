using InkySigma.Identity.ServiceProviders.HashProvider;
using InkySigma.Identity.ServiceProviders.RandomProvider;

namespace InkySigma.Identity.Model.Options
{
    public class PasswordOptions
    {
        public IPasswordHashProvider HashProvider { get; set; } = new Pbdf2HashProvider();
        public ISecureRandomProvider RandomProvider { get; set; } = new SecureRandomProvider();
    }
}
