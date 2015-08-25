using System;

namespace InkySigma.Identity.RandomProvider
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
