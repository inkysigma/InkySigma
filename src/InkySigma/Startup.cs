using InkySigma.Infrastructure.ApplicationBuilder;
using InkySigma.Infrastructure.ErrorHandler;
using InkySigma.Infrastructure.Filter;
using InkySigma.Infrastructure.Middleware;
using InkySigma.Infrastructure.ServiceBuilder;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;

namespace InkySigma
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IApplicationEnvironment Environment { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment app)
        {
            var builder = new ConfigurationBuilder(app.ApplicationBasePath)
                .AddJsonFile("config.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = app;
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.ConfigureMvcOptions();
            services.AddSqlConnectionBuilder();
        }

        public void Configure(IApplicationBuilder app)
        {
            // app.RequireSecure();
            app.UseMvc(ConfigureRoutes);
            app.UseCustomErrors();
            app.UseStaticFiles();
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "api/{controller}/{action}/{id?}");
        }
    }
}
