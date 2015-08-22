using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Dapper.Models
{
    public class UserClaimsViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public dynamic Payload { get; set; }
    }
}
