namespace InkySigma.Identity.ServiceProviders.RandomProvider
{
    public interface ISecureRandomProvider
    {
        byte[] GenerateRandom();
    }
}