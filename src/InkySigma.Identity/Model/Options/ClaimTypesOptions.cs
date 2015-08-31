using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InkySigma.Identity.Model.Options
{
    public class ClaimTypesOptions
    {
        public string AuthClaimType { get; set; } = ClaimTypes.Role;

        public string UserIdType { get; set; } = ClaimTypes.NameIdentifier;

        public string UserNameClaimType { get; set; } = ClaimTypes.Name;
    }
}
