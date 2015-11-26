using System;

namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public class TokenProvider : ITokenProvider
    {
        public string Generate()
        {
            var id = Guid.NewGuid();
            return id.ToString();
        }
    }
}