using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Repositories
{
    public interface IUserPropertyStore<TUser>:IDisposable where TUser : class
    {
    }
}
