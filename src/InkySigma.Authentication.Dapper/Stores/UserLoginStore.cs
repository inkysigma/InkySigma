using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserLoginStore : IUserLoginStore<User>
    {
        public SqlConnection Connection { get; }
        public bool IsDisposed { get; set; } = false;

        private string Table { get; }

        public UserLoginStore(SqlConnection connection, string table)
        {
            Connection = connection;
            Table = table;
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserLoginStore));
            Connection.Dispose();
            IsDisposed = true;
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserLoginStore));

        }

        public async Task<IEnumerable<TokenRow>> GetUserLoginsAsync(User user, CancellationToken token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(nameof(user));
            var result = await Connection.QueryAsync("SELECT * FROM @Table WHERE Id=@Id", new {user.Id, Table});
            if (result == null)
                throw new NullReferenceException(nameof(result));
            return result;
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
