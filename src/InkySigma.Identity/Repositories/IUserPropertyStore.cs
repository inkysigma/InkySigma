using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserPropertyStore<TUser>:IDisposable where TUser : class
    {
        Task<TUser> GetProperties(TUser user, CancellationToken token);
        Task<QueryResult> RemoveProperties(TUser user, CancellationToken token);
        Task<QueryResult> UpdateProperties(TUser user, CancellationToken token);
        Task<QueryResult> AddProperties(TUser user, CancellationToken token);
    }
}
