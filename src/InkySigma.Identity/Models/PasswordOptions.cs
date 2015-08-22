using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Identity.HashProvider;
using InkySigma.Identity.RandomProvider;

namespace InkySigma.Identity.Models
{
    public class PasswordOptions
    {
        public IPasswordHashProvider HashProvider { get; set; } = new Pbdf2HashProvider();
        public ISecureRandomProvider RandomProvider { get; set; } = new SecureRandomProvider();
    }
}
