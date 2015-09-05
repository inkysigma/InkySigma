using System;

namespace InkySigma.Identity.ServiceProviders.RandomProvider
{
    public class UserIdProvider : IUserIdProvider
    {
        public string Generate()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }
    }
}
