﻿using System.Collections.Generic;
using InkySigma.Authentication.Dapper.Models;

namespace InkySigma.Core
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SigmaUser : User
    {
        public virtual List<SigmaUser> Contacts { get; set; }
        public virtual Dictionary<SigmaUser, bool> ContactRequests { get; set; }
    }
}
