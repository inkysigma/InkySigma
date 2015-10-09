using System.Threading;

namespace InkySigma.Authentication.Obfuscator
{
    internal interface IObfuscator
    {
        string Obfuscate(string input, CancellationToken token);
    }
}