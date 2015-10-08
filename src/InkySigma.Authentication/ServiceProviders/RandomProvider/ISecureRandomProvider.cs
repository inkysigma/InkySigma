namespace InkySigma.Authentication.ServiceProviders.RandomProvider
{
    public interface ISecureRandomProvider
    {
        byte[] GenerateRandom();
    }
}