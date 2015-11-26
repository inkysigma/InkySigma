using System.Data.Common;
using InkySigma.Authentication.Dapper.Stores;
using InkySigma.Authentication.Model.Options;

namespace InkySigma.Authentication.Dapper.Models
{
    public class DefaultDapperRepositoryOptions<TUser> : RepositoryOptions<TUser> where TUser : User
    {
        public DefaultDapperRepositoryOptions(DbConnection connection, UserPropertyStore<TUser> PropertyStore)
        {
            UserStore = new UserStore<TUser>(connection);
            UserEmailStore = new UserEmailStore<TUser>(connection);
            UserRoleStore = new UserRoleStore<TUser>(connection);
            UserLockoutStore = new UserLockoutStore<TUser>(connection);
            UserLoginStore = new UserLoginStore<TUser>(connection);
            UserPasswordStore = new UserPasswordStore<TUser>(connection);
            UserTokenStore = new UserTokenStore<TUser>(connection);
        } 
    }
}
