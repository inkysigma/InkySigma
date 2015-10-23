using System;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    public class BasicAuthenticationMethod : IAuthenticationMethod
    {
        public UserTokenPair RetrieveUserTokenPair(HttpContext context)
        {
            var request = context.Request;
            var authorizationHeader = Encoding.UTF8.GetString(Convert.FromBase64String(request.Headers["Authorization"].ToString()));

            if (string.IsNullOrEmpty(authorizationHeader))
                return null;

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
        public static IServiceCollection AddBasicAuthentication(this IServiceCollection builder)
        {
            return builder.AddTransient<IAuthenticationMethod, BasicAuthenticationMethod>();
        }
    }
}
