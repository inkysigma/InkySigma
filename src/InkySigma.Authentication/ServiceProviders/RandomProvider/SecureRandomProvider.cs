using System.Security.Cryptography;

#if DNXCORE50
    using System.Security.Cryptography.RandomNumberGenerator
#endif

namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public class SecureRandomProvider : ISecureRandomProvider
    {
        private readonly int _length;
        public SecureRandomProvider(int length = 512)
        {
            _length = length;
        }
        public byte[] GenerateRandom()
        {
            using (var generator = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[_length];
                generator.GetBytes(buffer);
                return buffer;
            }
        }
    }
}
