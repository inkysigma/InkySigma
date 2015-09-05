using System;
using InkySigma.Identity.ServiceProviders.ClaimProvider;
using InkySigma.Identity.ServiceProviders.RandomProvider;

namespace InkySigma.Identity.Model.Options
{
    public class LoginManagerOptions<TUser> where TUser : class
    {
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
        public TimeSpan ExpirationTimeSpan { get; set; } = new TimeSpan(3, 0, 0, 0);
        public int AccessFailedCount { get; set; } = 5;

        internal IClaimsProvider<TUser> ClaimsProvider { get; set; }
    }
}
