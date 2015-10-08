using System;

namespace InkySigma.Authentication.ServiceProviders.RandomProvider
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
