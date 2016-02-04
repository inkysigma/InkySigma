using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Extensions.DependencyInjection;

namespace InkySigma.Authentication.AspNet
{
    public static class AuthenticationExtentions
    {
        /*
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
                    return new LoginService<User>(provider.GetService<UserManager<User>>(),
                        provider.GetService<ILogger<LoginService<User>>>(),
                        new LoginManagerOptions<User>
                        {
                            ClaimsProvider = new ClaimsProvider<User>(manager, new ClaimTypesOptions())
                        });
                });
            return services;
        }*/

        public static IAuthenticationBuilder<TUser> AddRepositories<TUser>(this IServiceCollection collection,
            RepositoryOptions<TUser> options) where TUser : class
        {
            var builder = new DefaultAuthenticationBuilder<TUser> {RepositoryOptions = options};
            return builder;
        }

        public static IAuthenticationBuilder<TUser> AddEmailProvider<TUser>(this IAuthenticationBuilder<TUser> builder,
            string host, string username, string password, string from, int port) where TUser : class
        {
            builder.EmailProvider = new EmailService(host, username, password, from, port);
            return builder;
        }

        public static IServiceCollection BuildManagers<TUser>(this IAuthenticationBuilder<TUser> builder) where TUser : class
        {
            builder.ServiceCollection.AddTransient(provider => new UserManager<TUser>(builder.RepositoryOptions, builder.EmailProvider, builder.UserLogger, builder.ExpirationTime));
            builder.ServiceCollection.AddTransient(
                provider =>
                {
                    var manager = provider.GetService<UserManager<TUser>>();
                    return new LoginService<TUser>(manager, builder.LoginLogger, builder.LoginOptions);
                });
            return builder.ServiceCollection;
        }
    }
}
