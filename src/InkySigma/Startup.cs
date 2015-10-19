using System;
using InkySigma.ApplicationBuilders;
using InkySigma.Authentication.AspNet.LoginMiddleware;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
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
        public Startup(IHostingEnvironment env, IApplicationEnvironment app)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = app;
        }

        public IConfiguration Configuration { get; set; }
        public IApplicationEnvironment Environment { get; set; }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.ConfigureMvcOptions();

            services.AddTransient<IEmailService, EmailService>(
                provider => new EmailService(Configuration["Email:Host"], Configuration["Email:UserName"],
                    Configuration["Email:Password"], Configuration["Email:From"], Int32.Parse(Configuration["Email:Port"])));

            services.AddSqlConnectionBuilder(Configuration["Data:Npgsql:ConnectionString"]);

            services.AddBasicAuthentication();

            services.AddDapperApplicationBuilder();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRemoveAspHeaders();

            app.RequireSecure();

            app.UseStaticFiles();

            app.UseAuthentication<User>();

            app.UseMvc(ConfigureRoutes);

            app.UseCustomErrors(Configuration["Application:Domain"]);
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "{controller}/{action}/{id?}");
        }
    }
}