using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserLoginStore : IUserLoginStore<User>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TokenRow>> GetUserLoginsAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasUserLoginAsync(User user, string token, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> AddUserLogin(User user, string userToken, string location, DateTime expiration, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> RemoveUserLogin(User user, string userToken, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> RemoveUser(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
