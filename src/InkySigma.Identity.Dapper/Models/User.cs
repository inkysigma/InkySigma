﻿using System.Collections.Generic;

namespace InkySigma.Identity.Dapper
{
    public class User
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public bool IsConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> Logins { get; set; }
    }
}