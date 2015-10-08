using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Authentication.Obfuscator
{
    interface IObfuscator
    {
        string Obfuscate(string input, CancellationToken token);
    }
}
