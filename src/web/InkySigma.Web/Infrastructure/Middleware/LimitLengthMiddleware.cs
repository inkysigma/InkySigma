using System.Threading.Tasks;
using InkySigma.Web.Model;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;

namespace InkySigma.Web.Infrastructure.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class LimitLengthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxLength;
        private readonly string _response;

        public LimitLengthMiddleware(int maxLength, string response)
        {
            _maxLength = maxLength;
            _response = response;
        }

        public LimitLengthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.ContentLength > _maxLength)
            {
                httpContext.Response.StatusCode = 400;
                if (!httpContext.Response.HasStarted)
                    await httpContext.Response.WriteAsync(_response);
                return;
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LimitLengthMiddlewareExtensions
    {
        public static IApplicationBuilder UseLimitLengthMiddleware(this IApplicationBuilder builder, int maxLength, string response)
        {
            return builder.UseMiddleware<LimitLengthMiddleware>(maxLength, response);
        }

        public static IApplicationBuilder UseLimitLengthMiddleware(this IApplicationBuilder builder, int maxLength)
        {
            var response = JsonConvert.SerializeObject(new StandardResponse
            {
                Code = 400,
                Information = maxLength.ToString(),
                Message = "The application has done something unexpected. Please contact your developer.",
                Succeeded = false
            });
            return builder.UseMiddleware<LimitLengthMiddleware>(maxLength, response);
        }
    }
}
