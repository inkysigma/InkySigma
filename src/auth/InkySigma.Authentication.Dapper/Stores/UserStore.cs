using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserStore<TUser> : IUserStore<TUser> where TUser : User
    {
        private readonly DbConnection _connection;
        public string Table { get; }

        public UserStore(DbConnection connection, string table = "auth.users")
        {
            _connection = connection;
            Table = table;
        }

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            _connection.Dispose();
            IsDisposed = true;
        }

        public async Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (user.UserName == null)
                throw new ArgumentNullException(nameof(user));
            var sqlResult =
                await
                    _connection.QueryAsync($"SELECT Id FROM {Table} WHERE UserName=@UserName",
                        new {table = Table, user.UserName});
            var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
            if (!enumerable.Any())
                return null;
            dynamic firstOrDefault = enumerable.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : null;
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user));
            var sqlResult =
                await _connection.QueryAsync($"SELECT UserName FROM {Table} WHERE Id=@Id", new {Key = user.Id});
            var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
            if (!arr.Any())
                return null;
            dynamic firstOrDefault = arr.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.UserName : null;
        }

        public async Task<string> GetNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (user.UserName == null && user.Id == null)
                throw new ArgumentNullException();
            if (user.UserName != null)
            {
                var sqlResult =
                    await
                        _connection.QueryAsync($"SELECT Name FROM {Table} WHERE UserName=@UserName",
                            new {table = Table, user.UserName});
                var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!enumerable.Any())
                    return null;
                dynamic firstOrDefault = enumerable.FirstOrDefault();
                return firstOrDefault != null ? firstOrDefault.Name : null;
            }
            else
            {
                var sqlResult =
                    await _connection.QueryAsync($"SELECT Name FROM {Table} WHERE Id=@Id", new {Key = user.Id});
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                return firstOrDefault != null ? firstOrDefault.Name : null;
            }
        }

        public async Task<TUser> FindUserByIdAsync(string id, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            dynamic value =
                (await
                    _connection.QueryAsync($"SELECT Id,Name,UserName FROM {Table} WHERE Id=@Id",
                        new {Id = id})).FirstOrDefault();
            if (value != null)
            {
                var user = new User
                {
                    Id = value.Id,
                    Name = value.Name,
                    UserName = value.UserName
                };
                return (TUser) user;
            }
            throw new InvalidUserException(id);
        }

        public async Task<TUser> FindUserByUserNameAsync(string name, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            dynamic value =
                (await
                    _connection.QueryAsync($"SELECT Id,Name,UserName FROM {Table} WHERE UserName=@Name",
                        new {table = Table, Name = name})).FirstOrDefault();
            if (value == null)
                throw new InvalidUserException(name);
            return (TUser) new User
            {
                Id = value.Id,
                Name = value.Name,
                UserName = value.UserName
            };
        }

        public async Task<IEnumerable<TUser>> FindUsersByNameAsync(string name, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            dynamic values =
                (await
                    _connection.QueryAsync($"SELECT Id,Name,UserName FROM {Table} WHERE Name=@name",
                        new {table = Table, name}));
            var users = new List<TUser>();
            foreach (var i in values)
            {
                users.Add((TUser) new User
                {
                    Id = i.Id,
                    Name = i.Name,
                    UserName = i.UserName
                });
            }
            return users;
        }

        public async Task<QueryResult> SetUserIdAsync(TUser user, string userid, CancellationToken token)
        {
            Header(token);
            if (user?.UserName == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(userid))
                throw new ArgumentNullException(nameof(userid));
            var succeeded = (await
                _connection.ExecuteAsync($"UPDATE {Table} SET Id=@id WHERE UserName=@username",
                    new {table = Table, username = user.UserName})) == 1;
            return new QueryResult
            {
                Succeeded = succeeded
            };
        }

        public async Task<QueryResult> SetUserNameAsync(TUser user, string username, CancellationToken token)
        {
            Header(token);
            if (user?.Id == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            var succeeded =
                (await
                    _connection.ExecuteAsync($"UPDATE {Table} SET UserName=@User WHERE Id=@Id",
                        new {table = Table, Key = user.Id, User = username})) == 1;
            return new QueryResult
            {
                Succeeded = succeeded
            };
        }

        public async Task<QueryResult> SetNameAsync(TUser user, string name, CancellationToken token)
        {
            Header(token);
            if (user != null && (user?.Id == null && user.UserName == null))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException();
            try
            {
                bool succeeded;
                if (user.Id != null)
                    succeeded =
                        (await
                            _connection.ExecuteAsync($"UPDATE {Table} SET Name=@name WHERE Id=@Id", new {table = Table})) ==
                        1;
                else
                    succeeded =
                        (await
                            _connection.ExecuteAsync($"UPDATE {Table} SET Name=@name WHERE UserName=@username",
                                new {table = Table, username = user.UserName, name})) ==
                        1;
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

        public async Task<QueryResult> SetUserActive(TUser user, bool isActive, CancellationToken token)
        {
            Header(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            try
            {
                var succeeded = (await _connection.ExecuteAsync($"UPDATE {Table} SET Active=@isActive WHERE Id=@Id", new
                {
                    user.Id,
                    table = Table,
                    isActive
                })) == 1;
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

        public async Task<QueryResult> AddUserAsync(TUser user, string guid, string name, string username,
            CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException(nameof(guid));
            var succeeded =
                (await
                    (_connection.ExecuteAsync(
                        $"INSERT INTO {Table} (Id,UserName,Name,Active) VALUES(@Id, @UserName, @Name, false)",
                        new {Id = guid, UserName = username, Name = name}))) == 1;
            return new QueryResult
            {
                Succeeded = succeeded
            };

        }

        public async Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var succeeded =
                    (await _connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id", new {user.Id})) == 1;
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

        public async Task<QueryResult> UpdateUserAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var succeeded =
                    await
                        _connection.ExecuteAsync($"UPDATE {Table} SET UserName=@UserName, Name=@Name WHERE Id=@Id",
                            new {table = Table, user.UserName, user.Name, user.Id}) == 1;
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
        
        public async Task<bool> HasUserIdAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user));
            return (await _connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@Id", new {Key = user.Id})).Count() == 1;
        }

        public async Task<bool> HasUserNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.UserName))
                throw new ArgumentNullException(nameof(user));
            return
                (await _connection.QueryAsync($"SELECT * FROM {Table} WHERE UserName=@UserName", new {user.UserName}))
                    .Count() == 1;
        }

        public async Task<bool> HasNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user));
            return
                (await
                    _connection.QueryAsync($"SELECT * FROM {Table} WHERE Name=@Name AND Id=@Id",
                        new {user.Name, Key = user.Id})).Count() == 1;
        }

        public async Task<bool> HasActivated(TUser user, CancellationToken token)
        {
            Header(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return (await _connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@Id AND Active=true")).Any();
        }

        private void Header(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserStore<TUser>));
        }

        private QueryResult BuildError(SqlException e)
        {
            var errors = (from object i in e.Errors
                select new QueryError
                {
                    Description = i.ToString()
                }).ToList();
            return new QueryResult
            {
                Errors = errors,
                Succeeded = false
            };
        }
    }
}