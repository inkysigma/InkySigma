﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Infrastructure.ErrorHandler;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace InkySigma
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment Environment { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment app)
        {
            var builder = new ConfigurationBuilder(app.ApplicationBasePath)
                .AddJsonFile("config.json");

            if (env.IsDevelopment())
                builder.AddUserSecrets();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc(ConfigureRoutes);
        }

        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

        public void ConfigureErrors(IApplicationBuilder builder, IHostingEnvironment env)
        {
            builder.UseErrorHandler(404, new RazorErrorPage("~/Views/Error/404.cshtml"), WebService.Mvc);
            builder.UseErrorHandler(503, new RazorErrorPage("~/Views/Error/503.cshtml"), WebService.Mvc);
            builder.UseErrorHandler()
        }
    }
}
