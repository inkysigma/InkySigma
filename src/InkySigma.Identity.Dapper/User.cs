using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Dapper
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class User
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public bool IsConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> Logins { get; set; } 
    }
}
