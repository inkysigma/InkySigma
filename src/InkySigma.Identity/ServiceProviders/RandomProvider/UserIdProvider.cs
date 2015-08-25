using System;

namespace InkySigma.Identity.RandomProvider
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
