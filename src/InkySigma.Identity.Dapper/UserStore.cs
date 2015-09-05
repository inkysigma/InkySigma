using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Identity.Dapper.Models;
using InkySigma.Identity.Model.Exceptions;
using InkySigma.Identity.Repositories;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity.Dapper
{
    public class UserStore : IUserStore<User>
    {
        private readonly SqlConnection _connection;
        private readonly string _table;

        public bool IsDisposed { get; set; } = false;

        public UserStore(SqlConnection connection, string table = "auth.users")
        {
            _connection = connection;
            _table = table;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            _connection.Dispose();
            IsDisposed = true;
        }

        private void Header(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserStore));
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.UserName == null)
                throw new ArgumentNullException(nameof(user));
            var sqlResult =
                await
                    _connection.QueryAsync("SELECT Id FROM @table WHERE UserName=@UserName",
                        new {table = _table, UserName = user.UserName});
            var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
            if (!enumerable.Any())
                return null;
            dynamic firstOrDefault = enumerable.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : null;
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.Key == null)
                throw new ArgumentNullException(nameof(user));
            var sqlResult =
                await _connection.QueryAsync("SELECT UserName FROM @table WHERE Id=@Key", new {user.Key});
            var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
            if (!arr.Any())
                return null;
            dynamic firstOrDefault = arr.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.UserName : null;
        }

        public async Task<string> GetNameAsync(User user, CancellationToken token)
        {
            Header(token);
            if (user.UserName == null && user.Key == null)
                throw new ArgumentNullException();
            if (user.UserName != null)
            {
                var sqlResult =
                    await
                        _connection.QueryAsync("SELECT Name FROM @table WHERE UserName=@UserName",
                            new {table = _table, UserName = user.UserName});
                var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!enumerable.Any())
                    return null;
                dynamic firstOrDefault = enumerable.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
            else
            {
                var sqlResult =
                    await _connection.QueryAsync("SELECT Name FROM @table WHERE Id=@Key", new {Key = user.Key});
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
        }

        public async Task<User> FindUserByIdAsync(string id, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            dynamic value =
                (await
                    _connection.QueryAsync(@"SELECT Id,Name,UserName FROM @table WHERE Id=@Id",
                        new {table = _table, Id = id})).FirstOrDefault();
            if (value != null)
            {
                var user = new User
                {
                    Key = value.Id,
                    Name = value.Name,
                    UserName = value.UserName
                };
                return user;
            }
            throw new InvalidUserException(id);
        }

        public async Task<User> FindUserByUserNameAsync(string name, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            dynamic value =
                (await
                    _connection.QueryAsync("SELECT Id,Name,UserName FROM @table WHERE UserName=@Name",
                        new {table = _table, Name = name})).FirstOrDefault();
            if (value == null)
                throw new InvalidUserException(name);
            return new User
            {
                Key = value.Id,
                Name = value.Name,
                UserName = value.UserName
            };
        }

        public async Task<IEnumerable<User>> FindUsersByNameAsync(string name, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            dynamic values =
                (await
                    _connection.QueryAsync("SELECT Id,Name,UserName FROM @table WHERE Name=@name",
                        new {table = _table, name}));
            var users = new List<User>();
            foreach (var i in values)
            {
                users.Add(new User
                {
                    Key = i.Id,
                    Name = i.Name,
                    UserName = i.UserName
                });
            }
            return users;
        }

        public async Task<QueryResult> SetUserIdAsync(User user, string userid, CancellationToken token)
        {
            Header(token);
            if (user?.UserName == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(userid))
                throw new ArgumentNullException(nameof(userid));
            try
            {
                bool succeeded = (await
                    _connection.ExecuteAsync("UPDATE @table SET Id=@id WHERE UserName=@username",
                        new {table = _table, username = user.UserName})) == 1;
                return new QueryResult()
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> SetUserNameAsync(User user, string username, CancellationToken token)
        {
            Header(token);
            if (user?.Key == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            try
            {
                bool succeeded =
                    (await
                        _connection.ExecuteAsync("UPDATE @table SET UserName=@User WHERE Id=@Key",
                            new {table = _table, Key = user.Key, User = username})) == 1;
                return new QueryResult
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> SetNameAsync(User user, string name, CancellationToken token)
        {
            Header(token);
            if (user != null && (user?.Key == null && user.UserName == null))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();
            try
            {
                bool succeeded;
                if (user.Key != null)
                    succeeded =
                        (await
                            _connection.ExecuteAsync("UPDATE @table SET Name=@name WHERE Id=@Key", new {table = _table})) ==
                        1;
                else
                    succeeded =
                        (await
                            _connection.ExecuteAsync("UPDATE @table SET Name=@name WHERE UserName=@username",
                                new {table = _table, username = user.UserName, name})) ==
                        1;
                return new QueryResult()
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> AddUserAsync(User user, string guid, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException(nameof(guid));
            try
            {
                bool succeeded =
                    (await
                        (_connection.ExecuteAsync(
                            "INSERT INTO @table (Id,UserName,Name) VALUES(@Id, @UserName, @Name)",
                            new {table = _table, Id = guid, UserName = user.UserName, Name = user.Name}))) == 1;
                return new QueryResult()
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> RemoveUserAsync(User user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.Key))
                throw new ArgumentNullException(nameof(user));
            try
            {
                bool succeeded =
                    (await _connection.ExecuteAsync("DELETE FROM @table WHERE Id=@Id", new {Id = user.Key})) == 1;
                return new QueryResult()
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> UpdateUserAsync(User user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.Key))
                throw new ArgumentNullException(nameof(user));
            try
            {
                bool succeeded =
                    await
                        _connection.ExecuteAsync("UPDATE @table SET UserName=@UserName, Name=@Name WHERE Id=@Id",
                            new {table = _table, user.UserName, user.Name, Id = user.Key}) == 1;
                return new QueryResult
                {
                    Succeeded = succeeded
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<bool> HasUserIdAsync(User user, CancellationToken token)
        {
            if (string.IsNullOrEmpty(user.Key))
                throw new ArgumentNullException(nameof(user));
            return (await _connection.QueryAsync("SELECT * FROM @table WHERE Id=@Key", new {user.Key})).Count() == 1;
        }

        public async Task<bool> HasUserNameAsync(User user, CancellationToken token)
        {
            if (string.IsNullOrEmpty(user.UserName))
                throw new ArgumentNullException(nameof(user));
            return
                (await _connection.QueryAsync("SELECT * FROM @table WHERE UserName=@UserName", new {user.UserName}))
                    .Count() == 1;
        }

        public async Task<bool> HasNameAsync(User user, CancellationToken token)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Key))
                throw new ArgumentNullException(nameof(user));
            return
                (await
                    _connection.QueryAsync("SELECT * FROM @table WHERE Name=@Name AND Id=@Key",
                        new {user.Name, user.Key})).Count() == 1;
        }

        private QueryResult BuildError(SqlException e)
        {
            var errors = (from object i in e.Errors
                select new QueryError()
                {
                    Description = i.ToString()
                }).ToList();
            return new QueryResult()
            {
                Errors = errors,
                Succeeded = false
            };
        }
    }
}

