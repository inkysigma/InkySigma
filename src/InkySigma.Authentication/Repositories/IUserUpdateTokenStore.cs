using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserUpdateTokenStore<in TUser> : IDisposable where TUser : class
    {
        Task<QueryResult> AddTokenAsync(TUser user, UpdateTokenRow token, CancellationToken cancellationToken);
        Task<IEnumerable<UpdateTokenRow>> GetTokensAsync(TUser user, CancellationToken token);
        Task<UpdateTokenRow> FindTokenAsync(TUser user, string code, CancellationToken cancellationToken);
        Task<QueryResult> RemoveTokenAsync(TUser user, string code, CancellationToken cancellationToken);
        Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken cancellationToken);
    }
}