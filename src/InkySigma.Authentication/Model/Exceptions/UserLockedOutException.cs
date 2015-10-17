using InkySigma.Common;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class UserLockedOutException : CommonException
    {
        public UserLockedOutException(string username) : base(401, "User is locked out.", username)
        {
            
        }
    }
}