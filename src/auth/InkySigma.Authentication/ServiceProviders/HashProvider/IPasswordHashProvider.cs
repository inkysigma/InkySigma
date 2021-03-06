﻿namespace InkySigma.Authentication.ServiceProviders.HashProvider
{
    public interface IPasswordHashProvider
    {
        string Hash(string password, byte[] salt);
        bool VerifyHash(string password, string provided, byte[] salt);
    }
}