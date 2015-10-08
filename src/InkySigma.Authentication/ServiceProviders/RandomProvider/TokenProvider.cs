using System;

namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public class TokenProvider : ITokenProvider
    {
        public string Generate()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }
    }
}
