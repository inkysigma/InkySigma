using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Authentication.AspNet.LoginMiddleware
{
    public class UserTokenPair
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
