using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Authentication.Model;

namespace InkySigma.Authentication.Dapper
{
    public static class UserAuthenticationManager
    {
        public static async Task<UpdateToken> AddUser<TUser>(this UserService<TUser> service, TUser user, CancellationToken token = default(CancellationToken))
            where TUser : User
        {
            var identity = await service.AddUserAsync(user, user.UserName, user.Name, token);
            user.Id = identity;
            await service.AddUserPasswordAsync(user, user.Password, token);
            await service.AddUserEmailAsync(user, user.Email, token);
            await service.AddUserRole(user, "User", token);
            await service.AddUserProperties(user, token);
            await service.AddUserLockout(user, token);
            var stoken = await service.RequestActivation(user, token);
            return stoken;
        }
    }
}