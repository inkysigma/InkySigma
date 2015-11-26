using System;
using System.Data.Common;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Dapper.Stores;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.ServiceProviders.ClaimProvider;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InkySigma.Authentication.Dapper
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddDapperApplicationBuilder(this IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var conn = provider.GetService<DbConnection>();
                var userStore = new UserStore<User>(conn);
                var repo = new RepositoryOptions<User>
                {
                    UserStore = userStore,
                    UserEmailStore = new UserEmailStore<User>(conn),
                    UserLockoutStore = new UserLockoutStore<User>(conn),
                    UserLoginStore = new UserLoginStore<User>(conn),
                    UserPasswordStore = new UserPasswordStore<User>(conn),
                    UserPropertyStore = new UserPropertyStore<User>(conn),
                    UserRoleStore = new UserRoleStore<User>(conn),
                    UserTokenStore = new UserTokenStore<User>(conn)
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

        public static IServiceCollection AddDapperApplicationBuilder<TUser>(this IServiceCollection services,
            IUserPropertyStore<TUser> propertyStore) where TUser : User
        {
            services.AddTransient(provider =>
            {
                var conn = provider.GetService<DbConnection>();
                var userStore = new UserStore<TUser>(conn);
                var repo = new RepositoryOptions<TUser>
                {
                    UserStore = userStore,
                    UserEmailStore = new UserEmailStore<TUser>(conn),
                    UserLockoutStore = new UserLockoutStore<TUser>(conn),
                    UserLoginStore = new UserLoginStore<TUser>(conn),
                    UserPasswordStore = new UserPasswordStore<TUser>(conn),
                    UserPropertyStore = propertyStore,
                    UserRoleStore = new UserRoleStore<TUser>(conn),
                    UserTokenStore = new UserTokenStore<TUser>(conn)
                };
                return new UserManager<TUser>(repo, provider.GetService<IEmailService>(),
                    provider.GetService<ILogger<UserManager<TUser>>>(), TimeSpan.FromDays(1));
            });
            services.AddTransient(
                provider =>
                {
                    var manager = provider.GetService<UserManager<TUser>>();
                    return new LoginManager<TUser>(manager,
                        provider.GetService<ILogger<LoginManager<TUser>>>(),
                        new LoginManagerOptions<TUser>
                        {
                            ClaimsProvider = new ClaimsProvider<TUser>(manager, new ClaimTypesOptions())
                        });
                });
            return services;
        }
    }
}
