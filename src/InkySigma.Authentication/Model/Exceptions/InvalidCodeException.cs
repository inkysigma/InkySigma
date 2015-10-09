using System;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidCodeException : Exception
    {
        public InvalidCodeException() : base("The given code was invalid.")
        {
            HResult = 5;
        }
    }
}