﻿namespace InkySigma.Identity.Options
{
    public class PasswordHashProviderOptions
    {
        public int Iterations { get; set; } = 5000;
        public int Length { get; set; } = 1024;
    }
}