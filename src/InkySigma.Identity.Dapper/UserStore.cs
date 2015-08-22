using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Dapper
{
    public class UserStore : IUserStore<User>
    {
        private readonly SqlConnection _connection;
        private readonly string _table; 

        public bool IsDisposed { get; set; } = false;

        public UserStore(SqlConnection connection, string table = "user")
        {
            _connection = connection;
            _table = table;
        }

        public void Dispose()
        {
            _connection.Dispose();
            IsDisposed = true;
        }

        private void Header(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(UserStore));
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.Email == null && user.UserName == null)
                throw new ArgumentNullException();
            if(user.UserName != null)
            {
                var sqlResult = await _connection.QueryAsync("SELECT Id FROM @table WHERE UserName=@UserName", new { table = _table, UserName = user.UserName});
                var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!enumerable.Any())
                    return null;
                dynamic firstOrDefault = enumerable.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Id;
                return null;
            }
            else
            {
                var sqlResult =
                    await _connection.QueryAsync("SELECT Id FROM @table WHERE Email=@Email", new {user.Email});
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Id;
                return null;
            }
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.Email == null && user.Key == null)
                throw new ArgumentNullException();
            if (user.Email != null)
            {
                var sqlResult = await _connection.QueryAsync("SELECT UserName FROM @table WHERE Email=@Email", new { table = _table, user.Email });
                var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!enumerable.Any())
                    return null;
                dynamic firstOrDefault = enumerable.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.UserName;
                return null;
            }
            else
            {
                var sqlResult =
                    await _connection.QueryAsync("SELECT UserName FROM @table WHERE Id=@Key", new { user.Key });
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.UserName;
                return null;
            }
        }

        public async Task<string> GetNameAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.Email == null && user.UserName == null && user.Key == null)
                throw new ArgumentNullException();
            if (user.UserName != null)
            {
                var sqlResult = await _connection.QueryAsync("SELECT Name FROM @table WHERE UserName=@UserName", new { table = _table, UserName = user.UserName });
                var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!enumerable.Any())
                    return null;
                dynamic firstOrDefault = enumerable.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
            else if (user.Email != null)
            {
                var sqlResult =
                    await _connection.QueryAsync("SELECT Name FROM @table WHERE Email=@Email", new {Email = user.Email});
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
            else
            {
                var sqlResult =
                    await _connection.QueryAsync("SELECT Name FROM @table WHERE Id=@Key", new { Key = user.Key });
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
        }

        public Task<User> FindUserByIdAsync(string id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByUserNameAsync(string name, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByNameAsync(string name, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetUserIdAsync(User user, string userid, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetUserNameAsync(User user, string username, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetNameAsync(User user, string name, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> AddUserAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> RemoveUserAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> UpdateUserAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasUserIdAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasUserNameAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasNameAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
