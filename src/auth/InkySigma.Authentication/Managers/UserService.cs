using System;
using System.Threading;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.ServiceProviders.EmailProvider;
using Microsoft.Extensions.Logging;

namespace InkySigma.Authentication.Managers
{
    public partial class UserService<TUser> : IDisposable where TUser : class
    {
        private readonly FormValidatorOptions _formOptions;
        private readonly ILogger _logger;
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
        public readonly IUserTokenStore<TUser> UserTokenStore;
        private bool _isDisposed;

        public UserService(RepositoryOptions<TUser> repositories, IEmailService emailService,
            ILogger logger, TimeSpan maxTokenTimeSpan = default(TimeSpan))
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

        public UserService(RepositoryOptions<TUser> repositories, FormValidatorOptions formOptions,
            PasswordOptions passwordOptions, RandomOptions randomOptions, IEmailService emailService,
            ILogger logger, TimeSpan timeSpan = default(TimeSpan))
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
            _timeSpan = timeSpan;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            UserEmailStore.Dispose();
            UserStore.Dispose();
            UserLockoutStore.Dispose();
            UserLoginStore.Dispose();
            UserPasswordStore.Dispose();
            UserRoleStore.Dispose();
            UserPropertyStore.Dispose();
            UserTokenStore.Dispose();
        }

        private void Handle(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserService<TUser>));
        }
    }
}