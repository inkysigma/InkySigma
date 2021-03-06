﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.Repositories;
using InkySigma.Common;

namespace InkySigma.Authentication.ServiceProviders.ClaimProvider
{
    public class ClaimsProvider<TUser> : IClaimsProvider<TUser> where TUser : class
    {
        public ClaimsProvider(IUserStore<TUser> userStore, IUserRoleStore<TUser> userRoleStore, ClaimTypesOptions options)
        {
            UserStore = userStore;
            UserRoleStore = userRoleStore;
            Options = options;
        }

        internal IUserStore<TUser> UserStore { get; set; }
        internal IUserRoleStore<TUser> UserRoleStore { get; set; }
        internal ClaimTypesOptions Options { get; set; }

        public async Task<ClaimsPrincipal> CreateAsync(TUser user, IEnumerable<string> roles, CancellationToken token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var enumerable = roles as string[] ?? roles.ToArray();
            if (!enumerable.Any())
                throw new ArgumentNullException(nameof(user));
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            token.ThrowIfCancellationRequested();

            var userName = await UserStore.GetUserNameAsync(user, token);
            var userId = await UserStore.GetUserIdAsync(user, token);

            var identity = new ClaimsIdentity();
            var claims = enumerable.CarefullyMerge(await UserRoleStore.GetUserRolesAsync(user, token));
            identity.AddClaim(new Claim(Options.UserIdType, userId));
            identity.AddClaim(new Claim(Options.UserNameClaimType, userName));
            foreach (var i in claims)
            {
                identity.AddClaim(new Claim(Options.AuthClaimType, i));
            }
            return new ClaimsPrincipal(identity);
        }
    }
}