using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserLockoutStore<in TUser>:IDisposable where TUser:class
    {
        Task<DateTime> GetLockoutEndDateTime(TUser user, CancellationToken token);
        Task<int> GetAccessFailedCount(TUser user, CancellationToken token);
        Task<bool> GetLockoutEnabled(TUser user, CancellationToken token);

        Task<QueryResult> AddUserLockout(TUser user, CancellationToken token);

        Task<QueryResult> RemoveUserLockout(TUser user, CancellationToken token);

        Task<QueryResult> SetLockoutEndDateTime(TUser user, DateTime dateTime, CancellationToken token);
        Task<QueryResult> SetLockoutEnabled(TUser user, bool isLockedOut, CancellationToken token);

        Task<int> IncrememntAccessFailedCount(TUser user, CancellationToken token);

        Task<QueryResult> ResetAccessFailedCount(TUser user, CancellationToken token);
    }
}
