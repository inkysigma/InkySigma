using System.Threading;

namespace InkySigma.Authentication.ServiceProviders.ObfuscationProvider
{
    internal interface IObfuscator
    {
        string Obfuscate(string input, CancellationToken token);
    }
}