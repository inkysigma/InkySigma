using System.IO;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;

namespace InkySigma.Authentication.Dapper.Infrastructure
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly long _length;
        private readonly LoginManager<User> _loginManager;
        private readonly UserManager<User> _manager;
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next, long length, UserManager<User> manager,
            LoginManager<User> loginManager)
        {
            _next = next;
            _length = length;
            _manager = manager;
            _loginManager = loginManager;
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
                    var principal = await _loginManager.VerifyToken(model.Username, model.Token);
                    if (principal != null)
                        httpContext.User = principal;
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