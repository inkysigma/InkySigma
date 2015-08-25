using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Identity.Model;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Repositories
{
    public interface IUserUpdateTokenStore <in TUser> where TUser : class
    {
        Task<QueryResult> AddTokenAsync(TUser user, string token, CancellationToken cancellationToken);

        Task<IEnumerable<UpdateTokenRow>> GetTokensAsync(TUser user, CancellationToken token);

        Task<QueryResult> RemoveTokenAsync(TUser user, string token, CancellationToken cancellationToken);
    }
}
