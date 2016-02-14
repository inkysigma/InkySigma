using System;
using System.Data.Common;
using InkySigma.Authentication.AspNet;
using InkySigma.Authentication.AspNet.LoginMiddleware;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.ClaimProvider;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Web.Core;
using InkySigma.Web.Infrastructure.ExceptionPage;
using InkySigma.Web.Infrastructure.Filters;
using InkySigma.Web.Infrastructure.Formatters;
using InkySigma.Web.Infrastructure.Middleware;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Npgsql;
using InkySigma.Web.Data;

namespace InkySigma.Web
{
    public class Startup
    {
        public Startup(IApplicationEnvironment app, ILoggerFactory factory)
        {
            var jsonConfig = new JsonConfigurationProvider("config.json");
            var environConfig = new EnvironmentVariablesConfigurationProvider();

            var builder = new ConfigurationBuilder();
            builder.Add(jsonConfig);
            builder.Add(environConfig);
            builder.SetBasePath(app.ApplicationBasePath);

            Configuration = builder.Build();
            Environment = app;

            Factory = factory;
        }

        public IConfiguration Configuration { get; set; }
        public IApplicationEnvironment Environment { get; set; }
        public ILoggerFactory Factory { get; set; }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var logger = Factory.CreateLogger("Web.Exception");
                options.Filters.Add(new ExceptionFilter(logger, new JsonExceptionPage()));
                options.InputFormatters.Add(new JsonInputFormatter());
                options.OutputFormatters.Add(new JsonStandardMediaTypeFormatter());
            });

            services.AddTransient<IEmailService, EmailService>(
                provider => new EmailService(Configuration["Email:Host"], Configuration["Email:UserName"],
                    Configuration["Email:Password"], Configuration["Email:From"],
                    Int32.Parse(Configuration["Email:Port"])));

            var connection = new NpgsqlConnection(Configuration["Data:Npgsql:ConnectionString"]);
            connection.OpenAsync();

            services.AddTransient<DbConnection>(provider => connection);

            services.AddBasicAuthentication();


            var repositoryConfig = new DefaultDapperRepositoryOptions<SigmaUser>(connection,
                new SigmaPropertyStore(connection));
            services
                .StartAuthenticationConfiguration<SigmaUser>()
                .AddRepositories(repositoryConfig)
                .AddEmailProvider(Configuration["Email:Host"], Configuration["Email:UserName"],
                    Configuration["Email:Password"], Configuration["Email:From"],
                    int.Parse(Configuration["Email:Port"]))
                .AddLoggers(Factory)
                .ConfigureLoginOptions(new LoginManagerOptions<SigmaUser>
                {
                    ClaimsProvider =
                        new ClaimsProvider<SigmaUser>(repositoryConfig.UserStore, repositoryConfig.UserRoleStore,
                            new ClaimTypesOptions())
                })
                .BuildManagers();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory factory)
        {

            app.RequireSecure();

            app.UseStaticFiles();

            app.UseAuthentication<SigmaUser>();

            app.UseMvc(ConfigureRoutes);

            app.UseRemoveAspHeaders();

            // app.UseCustomErrors(Configuration["Application:Domain"]);
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "{controller}/{action}/{id?}");
        }
    }
}