﻿using System;

namespace InkySigma.Authentication.Model.Exceptions
{
    /// <summary>
    ///     Represents an exception where a user with the specified Token or Password does not exist
    /// </summary>
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string username = null)
        {
            UserName = username;
        }

        public string UserName { get; set; }
    }
}