using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Identity.ServiceProviders.ClaimProvider
{
    public interface IClaimsProvider<in TUser> where TUser : class
    {
        Task<ClaimsPrincipal> CreateAsync(TUser user, IEnumerable<string> roles, CancellationToken token);
    }
}
