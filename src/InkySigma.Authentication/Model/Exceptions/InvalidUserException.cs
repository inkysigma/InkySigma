using InkySigma.Common;

namespace InkySigma.Authentication.Model.Exceptions
{
    /// <summary>
    ///     Represents an exception where a user with the specified Token or Password does not exist
    /// </summary>
    public class InvalidUserException : CommonException
    {
        public InvalidUserException(string username = null) : base(401, "The user given is incorrect", username)
        {
        }
    }
}