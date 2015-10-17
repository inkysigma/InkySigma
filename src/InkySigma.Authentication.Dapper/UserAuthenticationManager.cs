using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;

namespace InkySigma.Authentication.Dapper
{
    public static class UserAuthenticationManager
    {
        public static async Task<string> AddUser(this UserManager<User> manager, User user, CancellationToken token = default(CancellationToken))
        {
            var identity = await manager.AddUserAsync(user, user.UserName, token);
            user.Id = identity;
            await manager.AddUserPasswordAsync(user, user.Password, token);
            await manager.AddUserEmailAsync(user, user.Email, token);
            await manager.AddUserRole(user, "User", token);
            await manager.AddUserProperties(user, token);
            return identity;
        }
    }
}