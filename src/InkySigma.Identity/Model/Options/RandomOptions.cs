using InkySigma.Identity.ServiceProviders.RandomProvider;

namespace InkySigma.Identity.Model.Options
{
    public class RandomOptions
    {
        public IUserIdProvider UserIdProvider { get; set; } = new UserIdProvider();
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
    }
}
