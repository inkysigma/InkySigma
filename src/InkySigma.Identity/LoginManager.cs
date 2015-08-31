using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Model;
using InkySigma.Identity.Model.Exceptions;
using InkySigma.Identity.Model.Options;
using InkySigma.Identity.RandomProvider;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;
using InkySigma.Identity.ServiceProviders.ClaimProvider;
using Microsoft.Framework.Logging;

namespace InkySigma.Identity
{
    public class LoginManager<TUser> : IDisposable where TUser : class
    {
        private bool _isDisposed = false;

        internal UserManager<TUser> Users;
        internal IUserLoginStore<TUser> LoginStore;
        internal IUserLockoutStore<TUser> LockoutStore;  
        internal IClaimsProvider<TUser> ClaimsProvider; 
        internal ILogger<LoginManager<TUser>> Logger;
        internal ITokenProvider TokenProvider;
        internal TimeSpan ExpirationTimeSpan;
        internal int MaxCount;
        public LoginManager(UserManager<TUser> userManager, ILogger<LoginManager<TUser>> logger, LoginManagerOptions<TUser> options)
        {
            Users = userManager;
            LoginStore = userManager.UserLoginStore;
            LockoutStore = userManager.UserLockoutStore;
            Logger = logger;
            ClaimsProvider = options.ClaimsProvider;
            TokenProvider = options.TokenProvider;
            ExpirationTimeSpan = options.ExpirationTimeSpan;
            MaxCount = options.AccessFailedCount;
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

        /// <summary>
        /// Creates a token if the provided username and password match.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The plain text password</param>
        /// <param name="location">The location to associate with the token (IP or Computer Name is fine)</param>
        /// <param name="persistant"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A token that can be used to authenticate</returns>
        public async Task<string> CreateLogin(string username, string password, string location, bool persistant,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await Users.FindUserByUsername(username, cancellationToken);
            var exists = await Users.VerifyUserPasswordAsync(username, password, cancellationToken);
            if (!exists)
            {
                await LockoutStore.IncrememntAccessFailedCount(user, cancellationToken);
                throw new InvalidUserException(username);
            }

            if (await LockoutStore.GetLockoutEnabled(user, cancellationToken))
            {
                var endDateTime = await LockoutStore.GetLockoutEndDateTime(user, cancellationToken);
                if (endDateTime == null)
                {
                    await LockoutStore.RemoveUserLockout(user, cancellationToken);
                }
                else if (endDateTime < DateTime.Now)
                {
                    await LockoutStore.SetLockoutEnabled(user, false, cancellationToken);
                }
                else
                {
                    throw new InvalidUserException();
                }
            }

            var count = await LockoutStore.GetAccessFailedCount(user, cancellationToken);

            if (count > MaxCount)
            {
                await LockoutStore.ResetAccessFailedCount(user, cancellationToken);
                await LockoutStore.SetLockoutEnabled(user, true, cancellationToken);
                await LockoutStore.SetLockoutEndDateTime(user, DateTime.Now + ExpirationTimeSpan, cancellationToken);
                throw new UserLockedOutException();
            }

            var token = TokenProvider.Generate();
            await LockoutStore.ResetAccessFailedCount(user, cancellationToken);
            QueryResult result;
            if (!persistant)
                result =
                    await LoginStore.AddUserLogin(user, token, location, DateTime.Now + ExpirationTimeSpan, cancellationToken);
            else
                result = await LoginStore.AddUserLogin(user, token, location, DateTime.MaxValue, cancellationToken);
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

        public void Dispose()
        {
            Users.Dispose();
            LoginStore.Dispose();
        }
    }
}
