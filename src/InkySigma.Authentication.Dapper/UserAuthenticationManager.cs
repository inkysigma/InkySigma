using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;

namespace InkySigma.Authentication.Dapper
{
    public static class UserAuthenticationManager
    {
        public static async Task AddUser(this UserManager<User> manager, User user, CancellationToken token)
        {
            await manager.AddUserAsync(user, user.UserName, token);
        }
    }
}