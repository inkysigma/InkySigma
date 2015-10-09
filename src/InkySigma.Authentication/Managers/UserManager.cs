using System;
using System.Threading;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Framework.Logging;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser> : IDisposable where TUser : class
    {
        private readonly FormValidatorOptions _formOptions;
        private readonly ILogger<UserManager<TUser>> _logger;
        private readonly PasswordOptions _passwordOptions;
        private readonly RandomOptions _randomProvider;
        private readonly TimeSpan _timeSpan;
        public readonly IEmailService EmailService;
        public readonly IUserEmailStore<TUser> UserEmailStore;
        public readonly IUserLockoutStore<TUser> UserLockoutStore;
        public readonly IUserLoginStore<TUser> UserLoginStore;
        public readonly IUserPasswordStore<TUser> UserPasswordStore;
        public readonly IUserPropertyStore<TUser> UserPropertyStore;
        public readonly IUserRoleStore<TUser> UserRoleStore;
        public readonly IUserStore<TUser> UserStore;
        public readonly IUserUpdateTokenStore<TUser> UserTokenStore;
        private bool _isDisposed;

        public UserManager(RepositoryOptions<TUser> repositories, IEmailService emailService,
            ILogger<UserManager<TUser>> logger, TimeSpan maxTokenTimeSpan = default(TimeSpan))
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
            if (maxTokenTimeSpan == default(TimeSpan))
                maxTokenTimeSpan = TimeSpan.FromDays(1);
            _timeSpan = maxTokenTimeSpan;
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

        public void Dispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserManager<TUser>));
            _isDisposed = true;
            UserEmailStore.Dispose();
            UserStore.Dispose();
            UserLockoutStore.Dispose();
            UserLoginStore.Dispose();
            UserPasswordStore.Dispose();
            UserRoleStore.Dispose();
            UserPropertyStore.Dispose();
        }

        private void Handle(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserManager<TUser>));
        }
    }
}