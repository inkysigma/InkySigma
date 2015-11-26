namespace InkySigma.Web.ResponseModels
{
    public class LoginResponseModel
    {
        /// <summary>
        /// This represents whether the login was successful.
        /// This will fail if the username/password combination is incorrect.
        /// However, it will not specify which was incorrect. Only that one was.
        /// </summary>
        public bool LoginSucceeded { get; set; }

        /// <summary>
        /// This represents the name of the user. For more information, contact the other controllers
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// This is the token for all future requests that require authentication.
        /// </summary>
        public string Token { get; set; }
    }
}
