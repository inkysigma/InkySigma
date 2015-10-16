using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class AuthenticationBaseException : Exception
    {
        public int Code { get; set; }

        public AuthenticationBaseException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
