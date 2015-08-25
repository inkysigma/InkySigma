using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Model
{
    public class UpdateTokenRow
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UpdateProperty Property { get; set; } = UpdateProperty.Password;
    }

    public enum UpdateProperty
    {
        Password = 0
    }
}
