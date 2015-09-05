using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Model;
using InkySigma.Identity.Model.Exceptions;
using InkySigma.Identity.Model.Options;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;
using InkySigma.Identity.ServiceProviders.EmailProvider;
using Microsoft.Framework.Logging;

namespace InkySigma.Identity
{
    public class UserManager<TUser> : IDisposable where TUser : class
    {
        public readonly IUserStore<TUser> UserStore;
        public readonly IUserRoleStore<TUser> UserRoleStore;
        public readonly IUserLoginStore<TUser> UserLoginStore;
        public readonly IUserPasswordStore<TUser> UserPasswordStore;
        public readonly IUserLockoutStore<TUser> UserLockoutStore;
        public readonly IUserEmailStore<TUser> UserEmailStore;
        public readonly IUserPropertyStore<TUser> UserPropertyStore;
        public readonly IUserUpdateTokenStore<TUser> UserTokenStore;
        internal IEmailService EmailService;
        private readonly FormValidatorOptions _formOptions;
        private readonly PasswordOptions _passwordOptions;
        private readonly RandomOptions _randomProvider;
        private readonly ILogger<UserManager<TUser>> _logger;
        private bool _isDisposed = false;

        public UserManager(RepositoryOptions<TUser> repositories, IEmailService emailService,
            ILogger<UserManager<TUser>> logger)
        {
            UserStore = repositories.UserStore;
            UserRoleStore = repositories.UserRoleStore;
            UserPasswordStore = repositories.UserPasswordStore;
            UserLoginStore = repositories.UserLoginStore;
            UserLockoutStore = repositories.UserLockoutStore;
            UserEmailStore = repositories.UserEmailStore;
            UserPropertyStore = repositories.UserPropertyStore;
            UserTokenStore = repositories.UserTokenStore;
            _formOptions = new FormValidatorOptions();
            _passwordOptions = new PasswordOptions();
            _randomProvider = new RandomOptions();
            _logger = logger;
            EmailService = emailService;
        }

        public UserManager(RepositoryOptions<TUser> repositories, FormValidatorOptions formOptions,
            PasswordOptions passwordOptions, RandomOptions randomOptions, IEmailService emailService,
            ILogger<UserManager<TUser>> logger)
        {
            UserStore = repositories.UserStore;
            UserRoleStore = repositories.UserRoleStore;
            UserPasswordStore = repositories.UserPasswordStore;
            UserLoginStore = repositories.UserLoginStore;
            UserLockoutStore = repositories.UserLockoutStore;
            UserEmailStore = repositories.UserEmailStore;
            UserPropertyStore = repositories.UserPropertyStore;
            UserTokenStore = repositories.UserTokenStore;
            EmailService = emailService;
            _formOptions = formOptions;
            _passwordOptions = passwordOptions;
            _randomProvider = randomOptions;
            _logger = logger;
        }

        private void Handle(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserManager<TUser>));
        }

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

        public virtual async Task<QueryResult> AddUserUpdateToken(TUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var token = _randomProvider.TokenProvider.Generate();
            return await UserTokenStore.AddTokenAsync(user, token, cancellationToken);
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

        public async Task<TUser> FindUserById(string userId, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            Guid output;
            if (!Guid.TryParse(userId, out output))
                throw new FormatException("guid", new ArgumentException(userId));
            return await UserStore.FindUserByIdAsync(userId, token);
        }

        public async Task<TUser> FindUserByUsername(string username,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            return await UserStore.FindUserByUserNameAsync(username, token);
        }

        public async Task<string> GetUserEmailAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserEmailStore.GetUserEmailAsync(user, token);
        }

        public async Task<string> GetUserId(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            return await UserStore.GetUserIdAsync(user, token);
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserStore.GetUserNameAsync(user, token);
        }

        public async Task<string> GetNameAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserStore.GetNameAsync(user, token);
        }

        public async Task<TUser> GetUserProperties(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserPropertyStore.GetProperties(user, token);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(TUser user,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserRoleStore.GetUserRolesAsync(user, token);
        }

        public async Task<QueryResult> RemoveUserById(string userId,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();
            var tuser = await UserStore.FindUserByIdAsync(userId, token);
            return await RemoveUser(tuser, token);
        }

        public async Task<QueryResult> RemoveUser(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = QueryResult.Success();

            var IsAllRemoved = true;

            // Removes the user email
            result = result + await UserEmailStore.RemoveUserEmail(user, token);

            if (result.Succeeded)
                IsAllRemoved = false;

            // Removes the lockout
            result = result + await UserLockoutStore.RemoveUserLockout(user, token);
            if (result.Succeeded)
                IsAllRemoved = false;

            // Removes the password
            result = result + await UserPasswordStore.RemovePasswordAsync(user, token);
            if (result.Succeeded)
                IsAllRemoved = false;

            // Removes the logins
            result = result + await UserLoginStore.RemoveUser(user, token);
            if (result.Succeeded)
                IsAllRemoved = false;

            // Removes the key properties
            result = result + await UserPropertyStore.RemoveProperties(user, token);
            if (result.Succeeded)
                IsAllRemoved = false;

            // Don't remove key if any query failed.
            if (!result.Succeeded || IsAllRemoved)
                return result;

            result = await UserStore.RemoveUserAsync(user, token);
            if (!result.Succeeded)
                throw new InvalidUserException();
            return result;
        }

        public virtual async Task<QueryResult> RemoveUserUpdateToken(TUser user, string token,
            CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            return await UserTokenStore.RemoveTokenAsync(user, token, cancellationToken);
        }

        public async Task<QueryResult> RemoveUserRole(TUser user, string role,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new InvalidUserException();
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(token));
            return await UserRoleStore.RemoveUserRoleAsync(user, role, token);
        }

        public async Task<QueryResult> UpdatePassword(TUser user, string password,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            byte[] salt = _passwordOptions.RandomProvider.GenerateRandom();
            var hashed = _passwordOptions.HashProvider.Hash(password, salt);
            var result = await UserPasswordStore.SetPasswordAsync(user, hashed, CancellationToken.None);
            if (!result.Succeeded)
                return result;
            return await UserPasswordStore.SetSaltAsync(user, salt, CancellationToken.None);
        }

        public async Task<QueryResult> UpdateEmail(TUser user, string email,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            return await UserEmailStore.SetUserEmailAsync(user, email, CancellationToken.None);
        }

        public virtual async Task<QueryResult> ResetPassword(TUser user, string code, string password,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var result = await UserTokenStore.GetTokensAsync(user, token);
            var updateTokenRows = result as UpdateTokenRow[] ?? result.ToArray();
            if (!updateTokenRows.Any())
                throw new InvalidCodeException();
            foreach (var i in updateTokenRows.Where(i => i.Token == code))
            {
                if (i.Expiration > DateTime.Now)
                {
                    await RemoveUserUpdateToken(user, code, token);
                    throw new InvalidCodeException();
                }
                else
                {
                    var salt = _passwordOptions.RandomProvider.GenerateRandom();
                    var hashed = _passwordOptions.HashProvider.Hash(password, salt);
                    var query = await UserPasswordStore.SetPasswordAsync(user, hashed, token);
                    if (!query.Succeeded)
                        throw new ServerException();
                    return await UserPasswordStore.SetSaltAsync(user, salt, token);
                }
            }
            throw new InvalidCodeException();
        }

        public virtual async Task<bool> VerifyUserPasswordAsync(TUser user, string password,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            byte[] salt = await UserPasswordStore.GetSaltAsync(user, CancellationToken.None);
            string hashed = await UserPasswordStore.GetPasswordAsync(user, CancellationToken.None);
            var isCorrect = _passwordOptions.HashProvider.VerifyHash(hashed, password, salt);
            if (!isCorrect)
                return false;
            return true;
        }

        public virtual async Task<bool> VerifyUserPasswordAsync(string user, string password,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            var tuser = await UserStore.FindUserByUserNameAsync(user, CancellationToken.None);
            if (tuser == null)
                throw new InvalidUserException(user);
            return await VerifyUserPasswordAsync(tuser, password, token);
        }

        public virtual async Task<bool> RequestPasswordResetAsync(string user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException(nameof(user));
            var token = _randomProvider.TokenProvider.Generate();
            var etoken = Uri.EscapeDataString(token);
            return await EmailService.SendEmail(new EmailMessage()
            {
                Subject = "Password Reset",
                Body = $@"Go to http://www.inkysigma.com/Reset?user={Uri.EscapeDataString(user)}&token={etoken}"
            });
        }

        public void Dispose()
        {
            _isDisposed = true;
            UserEmailStore.Dispose();
            UserStore.Dispose();
            UserLockoutStore.Dispose();
            UserLoginStore.Dispose();
            UserPasswordStore.Dispose();
            UserRoleStore.Dispose();
            UserPropertyStore.Dispose();
        }
    }
}
