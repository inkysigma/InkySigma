using System;
using System.Linq;
using System.Runtime.CompilerServices;
using InkySigma.Identity.Models;

namespace InkySigma.Identity.HashProvider
{
    public class Pbdf2HashProvider : IPasswordHashProvider
    {
        private int _iterations;
        public Pbdf2HashProvider(PasswordHashProviderOptions options = null)
        {
            if(options == null)
                options = new PasswordHashProviderOptions();
            _iterations = options.Iterations;
        }

        public string Hash(string password, byte[] salt)
        {
            if(string.IsNullOrEmpty(password) || salt == null)
                throw new ArgumentNullException();
            throw new NotImplementedException();
        }

        public bool VerifyHash(string password, string provided, byte[] salt)
        {
            throw new NotImplementedException();
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
