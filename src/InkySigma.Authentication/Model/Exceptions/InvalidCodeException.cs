using System;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidCodeException : AuthenticationBaseException
    {
        public InvalidCodeException() : base(401, "The given code was invalid.")
        {
            HResult = 5;
        }
    }
}