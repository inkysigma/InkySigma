using System.Threading.Tasks;
using InkySigma.Authentication.Managers;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class AuthenticationMiddleware<TUser> where TUser : class
    {
        private readonly LoginService<TUser> _loginService;
        private readonly UserService<TUser> _service;
        private readonly RequestDelegate _next;
        private readonly IAuthenticationMethod _method;

        public AuthenticationMiddleware(RequestDelegate next, UserService<TUser> service,
            LoginService<TUser> loginService, IAuthenticationMethod method)
        {
            _next = next;
            _service = service;
            _loginService = loginService;
            _method = method;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var model = _method.RetrieveUserTokenPair(httpContext);
            if (model == null)
            {
                httpContext.User = null;
                await _next(httpContext);
                return;
            }
            var principal = await _loginService.VerifyToken(model.UserName, model.Token);

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