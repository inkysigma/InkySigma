using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.ClaimProvider;
using InkySigma.Identity.Exceptions;
using InkySigma.Identity.Model;
using InkySigma.Identity.Options;
using InkySigma.Identity.RandomProvider;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;
using Microsoft.Framework.Logging;

namespace InkySigma.Identity
{
    public class LoginManager<TUser> where TUser : class
    {
        private bool _isDisposed = false;

        internal UserManager<TUser> Users;
        internal IUserLoginStore<TUser> LoginStore;
        internal IClaimsProvider<TUser> ClaimsProvider; 
        internal ILogger<LoginManager<TUser>> Logger;
        internal ITokenProvider TokenProvider;
        internal TimeSpan ExpirationTimeSpan;
        public LoginManager(UserManager<TUser> userManager, IUserLoginStore<TUser> loginStore, IClaimsProvider<TUser> claimsProvider, ILogger<LoginManager<TUser>> logger, LoginManagerOptions options)
        {
            Users = userManager;
            LoginStore = loginStore;
            Logger = logger;
            ClaimsProvider = claimsProvider;
            TokenProvider = options.TokenProvider;
            ExpirationTimeSpan = options.ExpirationTimeSpan;
        }

        private void Handle(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(LoginManager<TUser>));
        }

        /// <summary>
        /// Verifies that a token exists and generates a ClaimsPrincipal off of it if it exists. If it doesnt, return null.
        /// If user doesn't exist, throws InvalidUserException if the user is not found
        /// </summary>
        /// <param name="username">The provided username (UTF-8 encoding)</param>
        /// <param name="token">The provided token</param>
        /// <param name="cancellationToken">Cancels the operation</param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> VerifyToken(string username, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            var user = await Users.FindUserByUsername(username, cancellationToken);

            if (user == null)
                throw new InvalidUserException(username);

            var tokens = await LoginStore.GetUserLoginsAsync(user, cancellationToken);
            var tokenRows = tokens as TokenRow[] ?? tokens.ToArray();

            if (!tokenRows.Any())
                return null;

            foreach (var i in tokenRows)
            {
                if (i.Token == token && i.UserName == username)
                {
                    if (i.Expiration > DateTime.Now)
                    {
                        await RemoveLogin(username, token, cancellationToken);
                        return null;
                    }
                    var role = await Users.GetUserRolesAsync(user, cancellationToken);
                    return await ClaimsProvider.CreateAsync(user, role, cancellationToken);
                }
            }

            return null;
        }

        public async Task<string> CreateLogin(string username, string password, bool persistant,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await Users.FindUserByUsername(username, cancellationToken);
            var exists = await Users.VerifyUserPasswordAsync(username, password, cancellationToken);
            if (!exists)
                return null;
            var token = TokenProvider.Generate();
            QueryResult result;
            if (!persistant)
                result =
                    await LoginStore.AddUserLogin(user, token, DateTime.Now + ExpirationTimeSpan, cancellationToken);
            else
                result = await LoginStore.AddUserLogin(user, token, DateTime.MaxValue, cancellationToken);
            return result.Succeeded ? token : null;
        }

        public async Task<QueryResult> RemoveLogin(string username, string token,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            var user = await Users.FindUserByUsername(username, cancellationToken);
            if (user == null)
                throw new InvalidUserException(username);
            return await LoginStore.RemoveUserLogin(user, token, cancellationToken);
        }
    }
}
