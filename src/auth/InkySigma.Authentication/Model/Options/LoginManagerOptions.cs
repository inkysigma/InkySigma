﻿using System;
using InkySigma.Authentication.ServiceProviders.ClaimProvider;
using InkySigma.Authentication.ServiceProviders.RandomProvider;

namespace InkySigma.Authentication.Model.Options
{
    public class LoginManagerOptions<TUser> where TUser : class
    {
        public ITokenProvider TokenProvider { get; set; } = new TokenProvider();
        public TimeSpan ExpirationTimeSpan { get; set; } = new TimeSpan(3, 0, 0, 0);
        public int AccessFailedCount { get; set; } = 5;
        public IClaimsProvider<TUser> ClaimsProvider { get; set; }
    }
}