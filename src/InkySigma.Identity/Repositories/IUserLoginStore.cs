using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    interface IUserLoginStore<in TUser>:IDisposable where TUser:class
    {
        Task<string[]> GetUserLogins(TUser user, CancellationToken token);

        Task<QueryResult> AddUserLogin(TUser user, string userToken, DateTime expiration, CancellationToken token);

        Task<QueryResult> RemoveUserLogin(TUser user, string userToken, CancellationToken token);
    }
}
