using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Model.Options
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
        public IUserUpdateTokenStore<TUser> UserTokenStore { get; set; } 
    }
}
