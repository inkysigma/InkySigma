namespace InkySigma.Identity.RandomProvider
{
    public interface ISecureRandomProvider
    {
        byte[] GenerateRandom();
    }
}