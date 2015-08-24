using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Options;
using InkySigma.Identity.RandomProvider;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity
{
    public class UserManager<TUser> : IDisposable where TUser : class
    {
        private readonly IUserStore<TUser> _userStore;
        private readonly IUserRoleStore<TUser> _userRoleStore;
        private readonly IUserLoginStore<TUser> _userLoginStore;
        private readonly IUserPasswordStore<TUser> _userPasswordStore;
        private readonly IUserLockoutStore<TUser> _userLockoutStore;
        private readonly IUserEmailStore<TUser> _userEmailStore;
        private readonly IUserPropertyStore<TUser> _userPropertyStore;
        private readonly FormValidatorOptions _formOptions;
        private readonly PasswordOptions _passwordOptions;
        private readonly RandomOptions _randomProvider;
        private bool _isDisposed = false;

        public UserManager(RepositoryOptions<TUser> repositories)
        {
            _userStore = repositories.UserStore;
            _userRoleStore = repositories.UserRoleStore;
            _userPasswordStore = repositories.UserPasswordStore;
            _userLoginStore = repositories.UserLoginStore;
            _userLockoutStore = repositories.UserLockoutStore;
            _userEmailStore = repositories.UserEmailStore;
            _userPropertyStore = repositories.UserPropertyStore;
            _formOptions = new FormValidatorOptions();
            _passwordOptions = new PasswordOptions();
            _randomProvider = new RandomOptions();
        }

        public UserManager(RepositoryOptions<TUser> repositories, FormValidatorOptions formOptions,
            PasswordOptions passwordOptions, RandomOptions randomOptions)
        {
            _userStore = repositories.UserStore;
            _userRoleStore = repositories.UserRoleStore;
            _userPasswordStore = repositories.UserPasswordStore;
            _userLoginStore = repositories.UserLoginStore;
            _userLockoutStore = repositories.UserLockoutStore;
            _userEmailStore = repositories.UserEmailStore;
            _userPropertyStore = repositories.UserPropertyStore;
            _formOptions = formOptions;
            _passwordOptions = passwordOptions;
            _randomProvider = randomOptions;
        }

        private void Handle()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserManager<TUser>));
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
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            var errors = _formOptions.UsernameValidator.Validate(username);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("username"));
            var guid = _randomProvider.UserIdProvider.Generate();
            var result = await _userStore.AddUserAsync(user, guid, token);
            return !result.Succeeded ? null : guid;
        }

        public virtual async Task<QueryResult> AddUserEmailAsync(TUser user, string email,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            var errors = _formOptions.EmailValidator.Validate(email);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("email"));
            return await _userEmailStore.AddUserEmailAsync(user, email, token);
        }

        public virtual async Task<QueryResult> AddLockoutAsync(TUser user,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException("user");
            return await _userLockoutStore.AddUserLockout(user, token);
        }

        public virtual async Task<QueryResult> AddUserPassword(TUser user, string password,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException("null");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            var errors = _formOptions.PasswordValidator.Validate(password);
            if (errors != null)
                throw new FormatException(errors.FirstOrDefault(), new ArgumentException("password"));
            var random = _passwordOptions.RandomProvider.GenerateRandom();
            var hashedPassword = _passwordOptions.HashProvider.Hash(password, random);
            return
                await
                    _userPasswordStore.AddPasswordAsync(user, hashedPassword, random,
                        token);
        }

        public virtual async Task<QueryResult> AddUserRole(TUser user, string role,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null || string.IsNullOrEmpty(role))
                throw new ArgumentNullException();
            return await _userRoleStore.AddUserRoleAsync(user, role, token);
        }

        public async Task<TUser> FindUserById(string userId, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            Guid output;
            if (userId == null)
                throw new ArgumentNullException();
            if (!Guid.TryParse(userId, out output))
                throw new ArgumentException();
            return await _userStore.FindUserByIdAsync(userId, token);
        }

        public async Task<TUser> FindUserByUsername(string username,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            return await _userStore.FindUserByUserNameAsync(username, token);
        }

        public async Task<string> GetUserEmailAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            return await _userEmailStore.GetUserEmailAsync(user, token);
        }

        public async Task<string> GetUserId(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            return await _userStore.GetUserIdAsync(user, token);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(TUser user,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            return await _userRoleStore.GetUserRolesAsync(user, token);
        }

        public async Task<QueryResult> RemoveUserById(string userId,
            CancellationToken token = default(CancellationToken))
        {
            Handle();
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();
            var tuser = await _userStore.FindUserByIdAsync(userId, token);
            return await RemoveUser(tuser);
        }

        public async Task<QueryResult> RemoveUser(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);

            if (user == null)
                throw new ArgumentNullException();

            var result = QueryResult.Success();

            // Removes the user email
            result = result + await _userEmailStore.RemoveUserEmail(user, token);

            // Removes the lockout
            result = result + await _userLockoutStore.RemoveUserLockout(user, token);

            // Removes the password
            result = result + await _userPasswordStore.RemovePasswordAsync(user, token);

            // Removes the logins
            result = result + await _userLoginStore.RemoveUser(user, token);

            // Don't remove key if any query failed.
            if (!result.Succeeded)
                return result;

            result = result + await _userStore.RemoveUserAsync(user, token);
            return result;
        }

        public async Task<QueryResult> RemoveUserRole(TUser user, string role, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException("token");
            return await _userRoleStore.RemoveUserRoleAsync(user, role, token);
        }

        public async Task<QueryResult> UpdatePassword(TUser user, string password,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            byte[] salt = _passwordOptions.RandomProvider.GenerateRandom();
            var hashed = _passwordOptions.HashProvider.Hash(password, salt);
            var result = await _userPasswordStore.SetPasswordAsync(user, hashed, CancellationToken.None);
            if (!result.Succeeded)
                return result;
            return await _userPasswordStore.SetSaltAsync(user, salt, CancellationToken.None);
        }

        public async Task<QueryResult> UpdateEmail(TUser user, string email,
            CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            return await _userEmailStore.SetUserEmailAsync(user, email, CancellationToken.None);
        }



        public virtual async Task<bool> VerifyUserPasswordAsync(TUser user, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            byte[] salt = await _userPasswordStore.GetSaltAsync(user, CancellationToken.None);
            string hashed = await _userPasswordStore.GetPasswordAsync(user, CancellationToken.None);
            var isCorrect = _passwordOptions.HashProvider.VerifyHash(hashed, password, salt);
            if (!isCorrect)
                return false;
            return true;
        }

        public virtual async Task<bool> VerifyUserPasswordAsync(string user, string password, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(user))
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            var tuser = await _userStore.FindUserByUserNameAsync(user, CancellationToken.None);
            return await VerifyUserPasswordAsync(tuser, password, token);
        }


        public void Dispose()
        {
            _isDisposed = true;
            _userEmailStore.Dispose();
            _userStore.Dispose();
            _userLockoutStore.Dispose();
            _userLoginStore.Dispose();
            _userPasswordStore.Dispose();
            _userRoleStore.Dispose();
            _userPropertyStore.Dispose();
        }
    }
}
