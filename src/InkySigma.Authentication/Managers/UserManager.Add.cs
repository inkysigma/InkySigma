using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;
using Microsoft.Framework.Logging;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser>
    {
        public virtual async Task<string> AddUserAsync(TUser user, string username,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new InvalidUserException(username);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            var errors = _formOptions.UsernameValidator.Validate(username);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("username"));
            var guid = _randomProvider.UserIdProvider.Generate();
            var result = await UserStore.AddUserAsync(user, guid, token);
            return !result.Succeeded ? null : guid;
        }

        public virtual async Task<QueryResult> AddUserEmailAsync(TUser user, string email,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
            {
                var exception = new ArgumentException(nameof(user));
                _logger.LogError(exception.HResult, exception.Message, exception);
                throw exception;
            }
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            var errors = _formOptions.EmailValidator.Validate(email);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("email"));
            return await UserEmailStore.AddUserEmailAsync(user, email, token);
        }

        public virtual async Task<QueryResult> AddLockoutAsync(TUser user,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserLockoutStore.AddUserLockout(user, token);
        }

        public virtual async Task<QueryResult> AddUserUpdatePasswordToken(TUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var token = _randomProvider.TokenProvider.Generate();
            return await UserTokenStore.AddTokenAsync(user, new UpdateTokenRow
            {
                Expiration = DateTime.Now + _timeSpan
            }, cancellationToken);
        }

        public virtual async Task<QueryResult> AddUserPassword(TUser user, string password,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            var errors = _formOptions.PasswordValidator.Validate(password);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("password"));
            var random = _passwordOptions.RandomProvider.GenerateRandom();
            var hashedPassword = _passwordOptions.HashProvider.Hash(password, random);
            return
                await
                    UserPasswordStore.AddPasswordAsync(user, hashedPassword, random,
                        token);
        }

        public virtual async Task<QueryResult> AddUserRole(TUser user, string role,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            return await UserRoleStore.AddUserRoleAsync(user, role, token);
        }

        public virtual async Task<QueryResult> AddUserProperties(TUser user,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserPropertyStore.AddProperties(user, token);
        }
    }
}