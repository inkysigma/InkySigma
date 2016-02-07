using System;

namespace InkySigma.Authentication.Model
{
    public class LoginToken
    {
        /// <summary>
        /// The location of the token. It can represent multiple different options such as IP or GPS coordinates.
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// The login token used to authenticate the user.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The expiration date of the token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}