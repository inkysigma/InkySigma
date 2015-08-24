using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Model
{
    public class TokenRow
    {
        public string UserName { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
    }
}
