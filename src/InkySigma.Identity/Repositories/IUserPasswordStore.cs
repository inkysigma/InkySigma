using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IPasswordStore<in TUser>:IDisposable where TUser:class
    {
        Task<string> GetPasswordAsync(TUser user, CancellationToken token);
        Task<QueryResult> AddPasswordAsync(TUser user, string password, CancellationToken token);
        Task<QueryResult> SetPasswordAsync(TUser user, string password, CancellationToken token);
        Task<bool> HasPasswordAsync(TUser user, CancellationToken token);
    }
}
