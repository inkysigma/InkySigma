using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.ServiceProviders.EmailProvider;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser>
    {
        public virtual async Task<QueryResult> ActivateAccount(TUser user, string code,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            var token = await UserTokenStore.FindTokenAsync(user, code, cancellationToken);
            if (token == null)
            {
                var result = new QueryResult
                {
                    Succeeded = false
                };
                result.Errors = new List<QueryError>();
                result.Errors.Add(new QueryError
                {
                    Code = "404",
                    Description = "No Token Found"
                });
                return result;
            }
            if (token.Expiration < DateTime.Now)
                return QueryResult.Fail("410", "The token has expired");
            if (token.Property != UpdateProperty.Activate)
                return QueryResult.Fail("400", "This token is not for activation");
            return await UserStore.SetUserActive(user, true, cancellationToken);
        }

        public virtual async Task<QueryResult> DeactivateAccount(TUser user, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await UserStore.SetUserActive(user, false, cancellationToken);
        }

        public virtual async Task<QueryResult> RequestActivation(TUser user, EmailMessage message, string guid,
            CancellationToken cancellationToken)
        {
            await EmailService.SendEmail(message);
            return await UserTokenStore.AddTokenAsync(user, new UpdateTokenRow
            {
                Expiration = DateTime.Now + _timeSpan,
                Property = UpdateProperty.Activate,
                Token = guid
            }, cancellationToken);
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
                var salt = _passwordOptions.RandomProvider.GenerateRandom();
                var hashed = _passwordOptions.HashProvider.Hash(password, salt);
                var query = await UserPasswordStore.SetPasswordAsync(user, hashed, token);
                if (!query.Succeeded)
                    throw new ServerException();
                return await UserPasswordStore.SetSaltAsync(user, salt, token);
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
            var salt = await UserPasswordStore.GetSaltAsync(user, CancellationToken.None);
            var hashed = await UserPasswordStore.GetPasswordAsync(user, CancellationToken.None);
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
            return await EmailService.SendEmail(new EmailMessage
            {
                Subject = "Password Reset",
                Body = $@"Go to http://www.inkysigma.com/Reset?user={Uri.EscapeDataString(user)}&token={etoken}"
            });
        }
    }
}