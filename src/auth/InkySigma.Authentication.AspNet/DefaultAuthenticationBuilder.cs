using System;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InkySigma.Authentication.AspNet
{
    public class DefaultAuthenticationBuilder<TUser> : IAuthenticationBuilder<TUser> where TUser : class
    {
        public IServiceCollection ServiceCollection { get; set; }
        public RepositoryOptions<TUser> RepositoryOptions { get; set; }
        public LoginManagerOptions<TUser> LoginOptions { get; set; }
        public IEmailService EmailProvider { get; set; }
        public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromDays(1);
        public ILogger UserLogger { get; set; }
        public ILogger LoginLogger { get; set; }
    }
}
