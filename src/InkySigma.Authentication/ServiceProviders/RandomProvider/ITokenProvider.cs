namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public interface ITokenProvider
    {
        string Generate();
    }
}
