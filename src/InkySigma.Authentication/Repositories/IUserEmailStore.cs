using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserEmailStore<TUser> : IDisposable where TUser : class
    {
        Task<string> GetUserEmailAsync(TUser user, CancellationToken token);
        Task<bool> GetUserEmailConfirmedAsync(TUser user, CancellationToken token);
        Task<QueryResult> AddUserEmailAsync(TUser user, string email, CancellationToken token);
        Task<QueryResult> RemoveUserEmail(TUser user, CancellationToken token);
        Task<QueryResult> SetUserEmailAsync(TUser user, string email, CancellationToken token);
        Task<QueryResult> SetUserEmailConfirmedAsync(TUser user, bool isConfirmed, CancellationToken token);
        Task<bool> HasUserEmailAsync(TUser user, CancellationToken token);
        Task<TUser> FindUserByEmailAsync(string email, CancellationToken token);
    }
}