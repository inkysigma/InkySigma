using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser>
    {
        public async Task<QueryResult> UpdatePassword(TUser user, string password,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();
            var salt = _passwordOptions.RandomProvider.GenerateRandom();
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

        public async Task<QueryResult> UpdateProperties(TUser user,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserPropertyStore.UpdateProperties(user, cancellationToken);
        }
    }
}