using System;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidCodeException : AuthenticationBaseException
    {
        public InvalidCodeException(string code = null) : base(401, "The given code was invalid.", code)
        {
        }
    }
}