using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Identity.RandomProvider;

namespace InkySigma.Identity.Options
{
    public class LoginManagerOptions
    {
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
        public TimeSpan ExpirationTimeSpan { get; set; } = new TimeSpan(3, 0, 0, 0);
    }
}
