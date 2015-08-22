using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserPasswordStore<in TUser>:IDisposable where TUser:class
    {
        Task<string> GetPasswordAsync(TUser user, CancellationToken token);
        Task<byte[]> GetSaltAsync(TUser user, CancellationToken token);

        Task<QueryResult> AddPasswordAsync(TUser user, string password, byte[] salt, CancellationToken token);

        Task<QueryResult> RemovePasswordAsync(TUser user, CancellationToken token);

        Task<QueryResult> SetPasswordAsync(TUser user, string password, CancellationToken token);
        Task<QueryResult> SetSaltAsync(TUser user, byte[] salt, CancellationToken token);
        Task<bool> HasPasswordAsync(TUser user, CancellationToken token);
    }
}
