using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace InkySigma.Infrastructure.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class RemoveAspHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public RemoveAspHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            httpContext.Response.Headers.Remove("X-Powered-By");
            httpContext.Response.Headers.Remove("Server");
            httpContext.Response.Headers.Remove("X-AspNet-Version");
            httpContext.Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RemoveAspHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseRemoveAspHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RemoveAspHeadersMiddleware>();
        }
    }
}
