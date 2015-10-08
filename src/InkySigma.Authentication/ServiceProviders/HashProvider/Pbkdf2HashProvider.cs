using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using InkySigma.Authentication.Model.Options;

namespace InkySigma.Authentication.ServiceProviders.HashProvider
{
    public class Pbdf2HashProvider : IPasswordHashProvider
    {
        private readonly int _iterations;
        private readonly int _length;
        public Pbdf2HashProvider(PasswordHashProviderOptions options = null)
        {
            if(options == null)
                options = new PasswordHashProviderOptions();
            _iterations = options.Iterations;
            _length = options.Length;
        }

        public string Hash(string password, byte[] salt)
        {
            if(string.IsNullOrEmpty(password) || salt == null)
                throw new ArgumentNullException();

            using (var hashProvider = new Rfc2898DeriveBytes(password, salt))
            {
                hashProvider.IterationCount = _iterations;
                return Convert.ToBase64String(hashProvider.GetBytes(_length));
            }
        }

        public bool VerifyHash(string password, string provided, byte[] salt)
        {
            if(string.IsNullOrEmpty(password)||string.IsNullOrEmpty(provided)||salt == null)
                throw new ArgumentNullException();

            if (password == Hash(provided, salt))
                return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public bool CompareByteArrays(byte[] a, byte[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null || a.Length != b.Length)
                return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }
    }
}
