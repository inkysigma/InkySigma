using System;
using System.IO;
using System.Threading.Tasks;
using InkySigma.Identity.Dapper.Models;
using InkySigma.Identity.Repositories;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InkySigma.Identity.Dapper.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _length;

        public AuthenticationMiddleware(RequestDelegate next, long length, UserManager<User> manager)
        {
            _next = next;
            _length = length;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.ContentLength > _length)
            {
                httpContext.Response.StatusCode = 400;
                return;
            }
            if (httpContext.Request.Method.ToLower() == "post" && httpContext.Request.ContentType == "application/json")
            {
                string body;
                using (var reader = new StreamReader(httpContext.Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }
                try
                {
                    var model = JsonConvert.DeserializeObject<UserClaimsViewModel>(body);
                }
                catch (JsonException)
                {
                    httpContext.Response.StatusCode = 400;
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
