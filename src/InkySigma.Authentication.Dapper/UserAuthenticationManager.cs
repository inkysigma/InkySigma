using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.ServiceProviders.EmailProvider;

namespace InkySigma.Authentication.Dapper
{
    public static class UserAuthenticationManager
    {
        public static async Task<UpdateTokenRow> AddUser(this UserManager<User> manager, User user, CancellationToken token = default(CancellationToken))
        {
            var identity = await manager.AddUserAsync(user, user.UserName, token);
            user.Id = identity;
            await manager.AddUserPasswordAsync(user, user.Password, token);
            await manager.AddUserEmailAsync(user, user.Email, token);
            await manager.AddUserRole(user, "User", token);
            await manager.AddUserProperties(user, token);
            await manager.AddUserLockout(user, token);
            var stoken = await manager.RequestActivation(user, token);
            return stoken;
        }
    }
}