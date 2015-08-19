using System.Threading.Tasks;
using Microsoft.AspNet.Builder;

namespace InkySigma.Infrastructure.ApplicationBuilder
{
    public static class IdentityBuilder
    {
        public static async Task<IApplicationBuilder> UseIdentity(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}
