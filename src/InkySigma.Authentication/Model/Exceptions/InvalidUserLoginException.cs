using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidUserLoginException : AuthenticationBaseException
    {
        public InvalidUserLoginException(string information) : base(401, "You aren't logged in.", information)
        {
        }
    }
}
