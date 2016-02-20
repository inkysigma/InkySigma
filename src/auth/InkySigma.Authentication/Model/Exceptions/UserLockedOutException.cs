using InkySigma.Common;
using InkySigma.Common.Exceptions;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class UserLockedOutException : CommonException
    {
        public UserLockedOutException(string username) : base(401, "User is locked out.", username, null)
        {
            
        }
    }
}