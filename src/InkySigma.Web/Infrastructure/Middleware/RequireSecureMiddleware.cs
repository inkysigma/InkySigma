using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace InkySigma.Web.Infrastructure.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class RequireSecureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequireSecureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.IsHttps)
            {
                var url = UriHelper.Encode("https", httpContext.Request.Host, httpContext.Request.PathBase,
                    httpContext.Request.Path, httpContext.Request.QueryString);
                httpContext.Response.Redirect(url, true);
                return;
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequireSecureMiddlewareExtensions
    {
        public static IApplicationBuilder RequireSecure(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequireSecureMiddleware>();
        }
    }
}