using System.Data.Common;
using InkySigma.Authentication.Dapper.Stores;
using InkySigma.Authentication.Model.Options;
using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Dapper.Models
{
    public class DefaultDapperRepositoryOptions<TUser> : RepositoryOptions<TUser> where TUser : User
    {
        public DefaultDapperRepositoryOptions(DbConnection connection, IUserPropertyStore<TUser> propertyStore)
        {
            UserStore = new UserStore<TUser>(connection);
            UserEmailStore = new UserEmailStore<TUser>(connection);
            UserRoleStore = new UserRoleStore<TUser>(connection);
            UserLockoutStore = new UserLockoutStore<TUser>(connection);
            UserLoginStore = new UserLoginStore<TUser>(connection);
            UserPasswordStore = new UserPasswordStore<TUser>(connection);
            UserTokenStore = new UserTokenStore<TUser>(connection);
            UserPropertyStore = propertyStore;
        } 
    }
}
