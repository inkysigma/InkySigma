using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Hosting.Server;
using Microsoft.AspNet.Http.Features.Internal;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace InkySigma.Console
{
    public class Program
    {
        private readonly IServiceProvider _provider;
        public Program(IServiceProvider provider)
        {
            _provider = provider;
        }
        public void Main(string[] args)
        {
            var tempBuilder = new ConfigurationBuilder().AddCommandLine(args);
            var tempConfig = tempBuilder.Build();
            var configFilePath = tempConfig["config"];

            var appBasPath = _provider.GetRequiredService<IApplicationEnvironment>().ApplicationBasePath;
            var builder = new ConfigurationBuilder(Path.Combine(appBasPath, "../InkySigma"));
            var config = builder.Build();

            var host = new WebHostBuilder(_provider, config).Build();

            using (host.Start())
            {
                
            }
        }
    }
}
