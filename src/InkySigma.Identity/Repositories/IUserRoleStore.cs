using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserRoleStore<in TUser>:IDisposable where TUser : class
    {
        Task<string[]> GetUserRolesAsync(TUser user, CancellationToken token);

        Task<QueryResult> AddUserRoleAsync(TUser user, string role, CancellationToken token);

        Task<QueryResult> RemoveUserRoleAsync(TUser user, string role, CancellationToken token);

        Task<QueryResult> RemoveUserRolesAsync(TUser user, CancellationToken token);

        Task<bool> HasRoleRoleAsync(TUser user, string role, CancellationToken token);
    }
}
