using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser>
    {
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

            var isAllRemoved = true;

            // Removes the user email
            result = result + await UserEmailStore.RemoveUser(user, token);

            if (result.Succeeded)
                isAllRemoved = false;

            // Removes the lockout
            result = result + await UserLockoutStore.RemoveUserLockout(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            // Removes the password
            result = result + await UserPasswordStore.RemovePasswordAsync(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            // Removes the logins
            result = result + await UserLoginStore.RemoveUser(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            // Removes the key properties
            result = result + await UserPropertyStore.RemoveProperties(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            result = result + await UserRoleStore.RemoveUserRolesAsync(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            result = result + await UserTokenStore.RemoveUserAsync(user, token);
            if (result.Succeeded)
                isAllRemoved = false;

            // Don't remove key if any query failed.
            if (!result.Succeeded || isAllRemoved)
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
    }
}