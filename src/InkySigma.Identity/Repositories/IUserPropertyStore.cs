using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace InkySigma.Identity.Repositories
{
    public interface IUserPropertyStore<TUser>:IDisposable where TUser : class
    {
        Task<TUser> GetProperties(TUser user);
        Task<TUser> RemoveProperties(TUser user);
        Task<TUser> UpdateProperties(TUser user);
        Task<TUser> AddProperties(TUser user);
    }
}
