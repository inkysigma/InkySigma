using System.Text;
using System.Threading;

namespace InkySigma.Authentication.ServiceProviders.ObfuscationProvider
{
    public class Obfuscator : IObfuscator
    {
        private readonly int _maxFiller;
        private readonly int _maxLength;

        public Obfuscator(int maxLength, int maxFiller)
        {
            _maxLength = maxLength;
            if (maxLength > maxFiller)
                maxFiller = maxLength + 1;
            _maxFiller = maxFiller;
        }

        public string Obfuscate(string input, CancellationToken token)
        {
            var shortened = input.Substring(0, _maxLength);
            var remainder = _maxFiller - input.Length;
            var builder = new StringBuilder();
            for (var i = 0; i < remainder; i++)
                builder.Append("*");
            return shortened + builder;
        }
    }
}