using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    interface IUserLockoutStore<in TUser>:IDisposable where TUser:class
    {
        Task<DateTime> GetLockoutEndDateTime(TUser user, CancellationToken token);
        Task<int> GetAccessFailedCount(TUser user, CancellationToken token);
        Task<int> GetLockoutEnabled(TUser user, CancellationToken token);

        Task<QueryResult> SetLockoutEndDateTime(TUser user, DateTime dateTime, CancellationToken token);
        Task<QueryResult> SetLockoutEnabled(TUser user, bool isLockedOut, CancellationToken token);

        Task<int> IncrememntAccessFailedCount(TUser user, CancellationToken token);

        Task<QueryResult> ResetAccessFailedCount(TUser user, CancellationToken token);
    }
}
