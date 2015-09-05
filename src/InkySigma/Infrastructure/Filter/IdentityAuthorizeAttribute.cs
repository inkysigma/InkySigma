using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using AuthorizationContext = Microsoft.AspNet.Mvc.AuthorizationContext;
using InkySigma.Identity.Dapper;
using Microsoft.AspNet.Http;

namespace InkySigma.Infrastructure.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class IdentityAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] Roles;
        public IdentityAuthorizeAttribute(params string[] roles)
        {
            Roles = roles;
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
                    if (!Roles.Contains(claim.Value))
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
