using System;
using System.IO;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;

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