using System;

namespace InkySigma.Identity.ServiceProviders.RandomProvider
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
