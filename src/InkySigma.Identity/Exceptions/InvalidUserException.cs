using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Exceptions
{
    /// <summary>
    /// Represents an exception where a user with the specified Token or Password does not exist
    /// </summary>
    public class InvalidUserException : Exception
    {
        public string UserName { get; set; }

        public InvalidUserException(string username = null)
        {
            UserName = username;
        }
    }
}
