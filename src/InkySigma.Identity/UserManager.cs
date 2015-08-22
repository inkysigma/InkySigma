using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Models;
using InkySigma.Identity.RandomProvider;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity
{
    public class UserManager<TUser>:IDisposable where TUser : class
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
            _randomProvider = new RandomOptions();
        } 

        public UserManager(RepositoryOptions<TUser> repositories, FormValidatorOptions formOptions, PasswordOptions passwordOptions, RandomOptions randomOptions)
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

        public virtual void Handle()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserManager<TUser>));
        }

        public virtual async Task<QueryResult> AddUserAsync(TUser user)
        {
           Handle();
           return await _userStore.AddUserAsync(user, CancellationToken.None);
        }

        public virtual async Task<QueryResult> AddUserEmailAsync(TUser user, string email)
        {
            Handle();
            return await _userEmailStore.AddUserEmailAsync(user, email, CancellationToken.None);
        }

        public virtual async Task<QueryResult> AddUserPassword(TUser user, string password)
        {
            Handle();
            var random = _passwordOptions.RandomProvider.GenerateRandom();
            var hashedPassword = _passwordOptions.HashProvider.Hash(password, random);
            return
                await
                    _userPasswordStore.AddPasswordAsync(user, hashedPassword, random,
                        CancellationToken.None);
        }

        public virtual async Task<QueryResult> AddUserRole(TUser user, string role)
        {
            return await _userRoleStore.AddUserRoleAsync(user, role, CancellationToken.None);
        }

        public virtual async Task<string> AddUserLoginAsync(TUser user, string password)
        {
            Handle();
            if (user == null || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            byte[] salt = await _userPasswordStore.GetSaltAsync(user, CancellationToken.None);
            string hashed = await _userPasswordStore.GetPasswordAsync(user, CancellationToken.None);
            var isCorrect = _passwordOptions.HashProvider.VerifyHash(hashed, password, salt);
            if (!isCorrect)
                return null;
            var token = _randomProvider.TokenProvider.Generate();
            var result = await _userLoginStore.AddUserLogin(user, token,
                DateTime.Now.Add(TimeSpan.FromDays(3)), CancellationToken.None);
            if (!result.Succeeded)
                throw new ApplicationException();
            return token;
        }

        public virtual async Task<string> AddUserLoginAsync(string user, string password)
        {
            Handle();
            if(string.IsNullOrEmpty(user)||string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            var tuser = await _userStore.FindUserByUserNameAsync(user, CancellationToken.None);
            return await AddUserLoginAsync(tuser, password);
        }

        public async Task<TUser> GetUserById(string userId)
        {
            Handle();
            Guid output;
            if (userId == null || !Guid.TryParse(userId, out output))
                return null;
            return await _userStore.FindUserByIdAsync(userId, CancellationToken.None);
        }

        public async Task<string> GetUserEmailAsync(TUser user)
        {
            return await _userEmailStore.GetUserEmailAsync(user, CancellationToken.None);
        }

        public async Task<string> GetUserId(TUser user)
        {
            return await _userStore.GetUserIdAsync(user, CancellationToken.None);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(TUser user)
        {
            return await _userRoleStore.GetUserRolesAsync(user, CancellationToken.None);
        }

        public async Task<QueryResult> RemoveUserById(string userId)
        {
            var tuser = await _userStore.FindUserByIdAsync(userId, CancellationToken.None);
            return await RemoveUser(tuser);
        }

        public async Task<QueryResult> RemoveUser(TUser user)
        {
            var result = QueryResult.Success();

            // Removes the user email
            result = result + await _userEmailStore.RemoveUserEmail(user, CancellationToken.None);

            // Removes the lockout
            result = result + await _userLockoutStore.RemoveUserLockout(user, CancellationToken.None);

            // Removes the password
            result = result + await _userPasswordStore.RemovePasswordAsync(user, CancellationToken.None);

            // Removes the logins
            result = result + await _userLoginStore.RemoveUserLogins(user, CancellationToken.None);

            // Don't remove key if any query failed.
            if (!result.Succeeded)
                return result;

            result = result + await _userStore.RemoveUserAsync(user, CancellationToken.None);
            return result;
        }

        public async Task<QueryResult> UpdateEmail(TUser user, string email)
        {
            return await _userEmailStore.SetUserEmailAsync(user, email, CancellationToken.None);
        }

        public async Task<bool> HasToken(string username, string token)
        {
            TUser user = await _userStore.FindUserByUserNameAsync(username, CancellationToken.None);
            return await HasToken(user, token);
        }

        public async Task<bool> HasToken(TUser user, string token)
        {
            var logins = await _userLoginStore.GetUserLogins(user, CancellationToken.None);
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
