using InkySigma.Authentication.ServiceProviders.RandomProvider;

namespace InkySigma.Authentication.Model.Options
{
    public class RandomOptions
    {
        public IUserIdProvider UserIdProvider { get; set; } = new UserIdProvider();
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
    }
}
