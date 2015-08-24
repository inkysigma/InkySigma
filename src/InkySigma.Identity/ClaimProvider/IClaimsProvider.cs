using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Identity.ClaimProvider
{
    public interface IClaimsProvider<in TUser> where TUser : class
    {
        Task<ClaimsPrincipal> CreateAsync(TUser user, IEnumerable<string> role, CancellationToken token);
    }
}
