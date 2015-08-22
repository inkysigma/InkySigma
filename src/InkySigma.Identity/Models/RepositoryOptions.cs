using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories;

namespace InkySigma.Identity.Models
{
    public class RepositoryOptions<TUser> where TUser : class
    {
        public IUserStore<TUser> UserStore { get; set; }
        public IUserRoleStore<TUser> UserRoleStore { get; set; }
        public IUserEmailStore<TUser> UserEmailStore { get; set; }
        public IUserPasswordStore<TUser> UserPasswordStore { get; set; }
        public IUserLoginStore<TUser> UserLoginStore { get; set; }
        public IUserLockoutStore<TUser> UserLockoutStore { get; set; }
        public IUserPropertyStore<TUser> UserPropertyStore { get; set; } 
    }
}
