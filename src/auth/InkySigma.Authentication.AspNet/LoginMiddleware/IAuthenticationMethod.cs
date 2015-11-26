using Microsoft.AspNet.Http;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    public interface IAuthenticationMethod
    {
        UserTokenPair RetrieveUserTokenPair(HttpContext context);
    }
}
