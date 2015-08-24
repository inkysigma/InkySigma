using System;
using InkySigma.Identity.RandomProvider;

namespace InkySigma.Identity.Model.Options
{
    public class LoginManagerOptions
    {
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
        public TimeSpan ExpirationTimeSpan { get; set; } = new TimeSpan(3, 0, 0, 0);
    }
}
