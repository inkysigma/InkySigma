using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Authentication.AspNet
{
    public interface IAuthenticationBuilder
    {
        DbConnection Connection { get; set; }
    }
}
