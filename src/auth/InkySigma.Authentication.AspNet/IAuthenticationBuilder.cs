﻿using System;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InkySigma.Authentication.AspNet
{
    public interface IAuthenticationBuilder<TUser> where TUser : class
    {
        IServiceCollection ServiceCollection { get; set; }
        
        RepositoryOptions<TUser> RepositoryOptions { get; set; }
        
        LoginManagerOptions<TUser> LoginOptions { get; set; }
        
        IEmailService EmailProvider { get; set; }

        TimeSpan ExpirationTime { get; set; }
        
        ILogger UserLogger { get; set; }
        
        ILogger LoginLogger { get; set; }
    }
}
