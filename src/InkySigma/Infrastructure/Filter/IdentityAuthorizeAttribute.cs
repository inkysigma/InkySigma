using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using AuthorizationContext = Microsoft.AspNet.Mvc.AuthorizationContext;

namespace InkySigma.Infrastructure.Filter
{
    public class IdentityAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            var user = context.HttpContext.User;
        }
    }
}
