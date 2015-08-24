using InkySigma.Identity.HashProvider;
using InkySigma.Identity.RandomProvider;

namespace InkySigma.Identity.Model.Options
{
    public class PasswordOptions
    {
        public IPasswordHashProvider HashProvider { get; set; } = new Pbdf2HashProvider();
        public ISecureRandomProvider RandomProvider { get; set; } = new SecureRandomProvider();
    }
}
