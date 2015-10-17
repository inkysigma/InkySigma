using System;

namespace InkySigma.Authentication.Model.Exceptions
{
    public class UserLockedOutException : AuthenticationBaseException
    {
        public UserLockedOutException(string username) : base(401, "User is locked out.", username)
        {
            
        }
    }
}