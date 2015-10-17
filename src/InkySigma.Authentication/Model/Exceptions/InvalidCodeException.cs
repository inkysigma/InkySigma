using InkySigma.Common;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidCodeException : CommonException
    {
        public InvalidCodeException(string code = null) : base(401, "The given code was invalid.", code)
        {
        }
    }
}