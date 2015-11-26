using System;

namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public class UserIdProvider : IUserIdProvider
    {
        public string Generate()
        {
            var id = Guid.NewGuid();
            return id.ToString();
        }
    }
}