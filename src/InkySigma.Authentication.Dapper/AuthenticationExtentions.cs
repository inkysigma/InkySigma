using System;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Dapper.Stores;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.ClaimProvider;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Npgsql;

namespace InkySigma.Authentication.Dapper
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddDapperApplicationBuilder(this IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var conn = provider.GetService<NpgsqlConnection>();
                var userStore = new UserStore(conn);
                var repo = new RepositoryOptions<User>
                {
                    UserStore = userStore,
                    UserEmailStore = new UserEmailStore(conn),
                    UserLockoutStore = new UserLockoutStore(conn),
                    UserLoginStore = new UserLoginStore(conn),
                    UserPasswordStore = new UserPasswordStore(conn),
                    UserPropertyStore = new UserPropertyStore(conn),
                    UserRoleStore = new UserRoleStore(conn),
                    UserTokenStore = new UserUpdateTokenStore(conn)
                };
                return new UserManager<User>(repo, provider.GetService<IEmailService>(),
                    provider.GetService<ILogger<UserManager<User>>>(), TimeSpan.FromDays(1));
            });
            services.AddTransient(
                provider =>
                {
                    var manager = provider.GetService<UserManager<User>>();
                    return new LoginManager<User>(provider.GetService<UserManager<User>>(),
                        provider.GetService<ILogger<LoginManager<User>>>(),
                        new LoginManagerOptions<User>
                        {
                            ClaimsProvider = new ClaimsProvider<User>(manager, new ClaimTypesOptions())
                        });
                });
            return services;
        }
    }
}
