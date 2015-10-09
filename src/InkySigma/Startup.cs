using InkySigma.ApplicationBuilder;
using InkySigma.Infrastructure.Middleware;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Routing;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;

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
            services.AddSqlConnectionBuilder(Configuration["Data:Npgsql:ConnectionString"]);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.RequireSecure();
            app.UseStaticFiles();
            app.UseMvc(ConfigureRoutes);
            app.UseCustomErrors(Configuration["Application:Domain"]);
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "{controller}/{action}/{id?}");
        }
    }
}
