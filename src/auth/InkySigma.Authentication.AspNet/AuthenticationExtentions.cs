using System;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using InkySigma.Authentication.ServiceProviders.RandomProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InkySigma.Authentication.AspNet
{
    public static class AuthenticationExtentions
    {
        public static IAuthenticationBuilder<TUser> StartAuthenticationConfiguration<TUser>(
            this IServiceCollection collection) where TUser : class
        {
            var builder = new DefaultAuthenticationBuilder<TUser> {ServiceCollection = collection};
            return builder;
        }

        public static IAuthenticationBuilder<TUser> AddRepositories<TUser>(this IAuthenticationBuilder<TUser> builder,
            RepositoryOptions<TUser> options) where TUser : class
        {
            builder.RepositoryOptions = options;
            return builder;
        }

        public static IAuthenticationBuilder<TUser> AddEmailProvider<TUser>(this IAuthenticationBuilder<TUser> builder,
            string host, string username, string password, string from, int port) where TUser : class
        {
            builder.EmailProvider = new EmailService(host, username, password, from, port);
            return builder;
        }

        public static IAuthenticationBuilder<TUser> AddLoggers<TUser>(this IAuthenticationBuilder<TUser> builder,
            ILoggerFactory loggerFactory) where TUser : class
        {
            builder.LoginLogger = loggerFactory.CreateLogger("Authentication.LoginService");
            builder.UserLogger = loggerFactory.CreateLogger("Authentication.UserService");
            return builder;
        }

        public static IAuthenticationBuilder<TUser> ConfigureLoginOptions<TUser>(
            this IAuthenticationBuilder<TUser> builder, LoginManagerOptions<TUser> options) where TUser : class
        {
            builder.LoginOptions = options;
            return builder;
        }

        public static IServiceCollection BuildManagers<TUser>(this IAuthenticationBuilder<TUser> builder)
            where TUser : class
        {
            if (builder.LoginOptions == null)
                throw new ArgumentNullException(nameof(builder.LoginOptions));
            if (builder.LoginLogger == null)
                throw new ArgumentNullException(nameof(builder.LoginLogger));
            if (builder.RepositoryOptions == null)
                throw new ArgumentNullException(nameof(builder.RepositoryOptions));
            if (builder.EmailProvider == null)
                throw new ArgumentNullException(nameof(builder.EmailProvider));
            if (builder.ServiceCollection == null)
                throw new ArgumentNullException(nameof(builder.ServiceCollection));
            if (builder.UserLogger == null)
                throw new ArgumentNullException(nameof(builder));
            builder.ServiceCollection.AddTransient(
                provider =>
                    new UserManager<TUser>(builder.RepositoryOptions, builder.EmailProvider, builder.UserLogger,
                        builder.ExpirationTime));
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
