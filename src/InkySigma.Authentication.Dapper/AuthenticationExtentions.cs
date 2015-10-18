using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Framework.DependencyInjection;

namespace InkySigma.Authentication.Dapper
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddDapperApplicationBuilder(this IServiceCollection services)
        {
            var repo = new RepositoryOptions<User>
            {

            };
            services.AddTransient(provider =>
            {
                return new UserManager<User>(repo, provider.GetService<IEmailService>() );
            });
            return services;
        }
    }
}
