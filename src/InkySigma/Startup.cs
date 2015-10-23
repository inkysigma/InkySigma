using System;
using System.Diagnostics;
using InkySigma.ApplicationBuilders;
using InkySigma.Authentication.AspNet.LoginMiddleware;
using InkySigma.Authentication.Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Infrastructure.ExceptionPage;
using InkySigma.Infrastructure.Filters;
using InkySigma.Infrastructure.Middleware;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Routing;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Configuration.EnvironmentVariables;
using Microsoft.Framework.Configuration.Json;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace InkySigma
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment app)
        {
            Debug.WriteLine(env.MapPath("~"));
            var jsonConfig = new JsonConfigurationProvider("config.json");
            var environConfig = new EnvironmentVariablesConfigurationProvider();

            var builder = new ConfigurationBuilder(jsonConfig, environConfig);
            builder.SetBasePath(app.ApplicationBasePath);
            Configuration = builder.Build();
            Environment = app;
        }

        public IConfiguration Configuration { get; set; }
        public IApplicationEnvironment Environment { get; set; }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var logger = new Logger<ExceptionContext>(new LoggerFactory());
                options.Filters.Add(new ExceptionFilter(logger, new JsonExceptionPage()));
            });

            services.AddTransient<IEmailService, EmailService>(
                provider => new EmailService(Configuration["Email:Host"], Configuration["Email:UserName"],
                    Configuration["Email:Password"], Configuration["Email:From"], Int32.Parse(Configuration["Email:Port"])));

            services.AddSqlConnectionBuilder(Configuration["Data:Npgsql:ConnectionString"]);

            services.AddBasicAuthentication();

            services.AddDapperApplicationBuilder();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.RequireSecure();

            app.UseStaticFiles();

            app.UseAuthentication<User>();

            app.UseMvc(ConfigureRoutes);

            app.UseRemoveAspHeaders();

            app.UseCustomErrors(Configuration["Application:Domain"]);
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "api/{controller}/{action}/{id?}");
        }
    }
}