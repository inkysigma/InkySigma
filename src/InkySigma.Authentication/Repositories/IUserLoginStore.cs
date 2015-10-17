using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserLoginStore<in TUser> : IDisposable where TUser : class
    {
        Task<IEnumerable<TokenRow>> GetUserLoginsAsync(TUser user, CancellationToken cancellationToken);
        Task<bool> HasUserLoginAsync(TUser user, string token, CancellationToken cancellationToken);

        Task<QueryResult> AddUserLogin(TUser user, string token, string location, DateTime expiration,
            CancellationToken cancellationToken);

        Task<QueryResult> RemoveUserLogin(TUser user, string token, CancellationToken cancellationToken);
        Task<QueryResult> RemoveUser(TUser user, CancellationToken cancellationToken);
    }
}