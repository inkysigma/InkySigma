﻿using System;

namespace InkySigma.Identity.Model
{
    public class TokenRow
    {
        public string UserName { get; set; }
        public string Location { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
    }
}