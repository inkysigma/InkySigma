using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserRoleStore : IUserRoleStore<User>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetUserRolesAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> AddUserRoleAsync(User user, string role, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> RemoveUserRoleAsync(User user, string role, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> RemoveUserRolesAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRoleRoleAsync(User user, string role, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
