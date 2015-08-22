using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserLoginStore<in TUser>:IDisposable where TUser:class
    {
        Task<string[]> GetUserLogins(TUser user, CancellationToken token);

        Task<QueryResult> AddUserLogin(TUser user, string userToken, DateTime expiration, CancellationToken token);

        Task<QueryResult> RemoveUserLogin(TUser user, string userToken, CancellationToken token);

        Task<QueryResult> RemoveUserLogins(TUser user, CancellationToken token);
    }
}
