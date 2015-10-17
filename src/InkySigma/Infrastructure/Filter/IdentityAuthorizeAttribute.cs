using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace InkySigma.Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class IdentityAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] _roles;

        public IdentityAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            var user = context.HttpContext.User;
            var httpContext = context.HttpContext;
            if (user == null)
            {
                Challenge(httpContext);
                return;
            }
            foreach (var claim in user.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    if (!_roles.Contains(claim.Value))
                    {
                        Challenge(httpContext);
                        return;
                    }
                    base.OnAuthorization(context);
                }
            }
        }

        private void Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
        }
    }
}