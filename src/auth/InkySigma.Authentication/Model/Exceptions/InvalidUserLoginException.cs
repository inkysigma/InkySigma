using InkySigma.Common;
using InkySigma.Common.Exceptions;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidUserLoginException : CommonException
    {
        public InvalidUserLoginException(string information) : base(401, "You aren't logged in.", information, "An invalid user login was given.")
        {
        }
    }
}
