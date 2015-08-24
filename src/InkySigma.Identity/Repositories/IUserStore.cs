using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserStore<TUser>:IDisposable where TUser : class
    {
        Task<string> GetUserIdAsync(TUser user, CancellationToken token);
        Task<string> GetUserNameAsync(TUser user, CancellationToken token);
        Task<string> GetNameAsync(TUser user, CancellationToken token);

        Task<TUser> FindUserByIdAsync(string id, CancellationToken token);
        Task<TUser> FindUserByUserNameAsync(string name, CancellationToken token);
        Task<TUser> FindUserByNameAsync(string name, CancellationToken token);

        Task<QueryResult> SetUserIdAsync(TUser user, string userid, CancellationToken token);
        Task<QueryResult> SetUserNameAsync(TUser user, string username, CancellationToken token);
        Task<QueryResult> SetNameAsync(TUser user, string name, CancellationToken token);

        Task<QueryResult> AddUserAsync(TUser user, string guid, CancellationToken token);
        Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken token);
        Task<QueryResult> UpdateUserAsync(TUser user, CancellationToken token);

        Task<bool> HasUserIdAsync(TUser user, CancellationToken token);
        Task<bool> HasUserNameAsync(TUser user, CancellationToken token);
        Task<bool> HasNameAsync(TUser user, CancellationToken token);
    }
}