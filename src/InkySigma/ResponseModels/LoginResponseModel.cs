﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.ResponseModels
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
        /// This is the token Id
        /// </summary>
        public string Token { get; set; }
    }
}