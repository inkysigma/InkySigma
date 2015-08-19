using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Models;

namespace InkySigma.Identity.Repositories
{
    public interface IUserStore<TUser>:IDisposable where TUser : class
    {
        Task<string> GetUserIdAsync(TUser user, CancellationToken token);
        Task<string> GetUserNameAsync(TUser user, CancellationToken token);

        Task<TUser> FindUserByIdAsync(string id, CancellationToken token);
        Task<TUser> FindUserByUserNameAsync(string name, CancellationToken token);

        Task<QueryResult> SetUserNameAsync(string username, TUser user, CancellationToken token);

        Task<QueryResult> AddUserAsync(TUser user, CancellationToken token);
        Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken token);
        Task<QueryResult> UpdateUserAsync(TUser user, CancellationToken token);
    }
}