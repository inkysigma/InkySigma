using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using InkySigma.Common;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    public class BasicAuthenticationMethod : IAuthenticationMethod
    {
        public UserTokenPair RetrieveUserTokenPair(HttpContext context)
        {
            var request = context.Request;
            var authorizationHeader = Encoding.UTF8.GetString(Convert.FromBase64String(request.Headers["Authorization"].ToString()));

            Debug.WriteLine(authorizationHeader);
            var authHeaderArrary = authorizationHeader.Split(':');
            if (authHeaderArrary.Length != 2)
                throw new HeaderFormatException(400, "Authorization");
            return new UserTokenPair
            {
                UserName = authHeaderArrary[0],
                Token = authHeaderArrary[1]
            };
        }
    }

    public static class BasicAutheitcationMethodMiddleware
    {
        public static IServiceCollection UseBasicAuthentication(this IServiceCollection builder)
        {
            return builder.AddTransient<IAuthenticationMethod, BasicAuthenticationMethod>();
        }
    }
}
