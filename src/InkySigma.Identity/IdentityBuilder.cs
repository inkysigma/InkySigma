using System.Threading.Tasks;
using Microsoft.AspNet.Builder;

namespace InkySigma.Identity
{
    public static class IdentityBuilder
    {
        public static IApplicationBuilder UseIdentity(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}
