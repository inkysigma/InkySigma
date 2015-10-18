using System.IO;
using System.Threading.Tasks;
using InkySigma.Authentication.Managers;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class AuthenticationMiddleware<TUser> where TUser : class
    {
        private readonly LoginManager<TUser> _loginManager;
        private readonly UserManager<TUser> _manager;
        private readonly RequestDelegate _next;
        private readonly IAuthenticationMethod _method;

        public AuthenticationMiddleware(RequestDelegate next, UserManager<TUser> manager,
            LoginManager<TUser> loginManager, IAuthenticationMethod method)
        {
            _next = next;
            _manager = manager;
            _loginManager = loginManager;
            _method = method;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var model = _method.RetrieveUserTokenPair(httpContext);
            var principal = await _loginManager.VerifyToken(model.UserName, model.Token);

            if (principal != null)
                httpContext.User = principal;

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthentication<TUser>(this IApplicationBuilder builder) where TUser : class
        {
            return builder.UseMiddleware<AuthenticationMiddleware<TUser>>();
        }
    }
}