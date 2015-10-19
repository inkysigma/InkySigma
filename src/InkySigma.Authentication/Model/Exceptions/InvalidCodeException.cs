using InkySigma.Common;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class InvalidCodeException : CommonException
    {
        public InvalidCodeException(string code = null, int errorCode = 401) : base(errorCode, "The given code was invalid.", code, null)
        {
        }
    }
}