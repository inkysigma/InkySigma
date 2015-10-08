using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Authentication.Managers
{
    public partial class UserManager<TUser>
    {
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
    }
}
