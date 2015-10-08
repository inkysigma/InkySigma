using InkySigma.Authentication.ServiceProviders.HashProvider;
using InkySigma.Authentication.ServiceProviders.RandomProvider;

namespace InkySigma.Authentication.Model.Options
{
    public class PasswordOptions
    {
        public IPasswordHashProvider HashProvider { get; set; } = new Pbdf2HashProvider();
        public ISecureRandomProvider RandomProvider { get; set; } = new SecureRandomProvider();
    }
}
