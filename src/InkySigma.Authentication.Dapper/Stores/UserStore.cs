﻿using System;
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
        private readonly string _table;

        public UserStore(DbConnection connection, string table = "auth.users")
        {
            _connection = connection;
            _table = table;
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
                    _connection.QueryAsync("SELECT Id FROM @table WHERE UserName=@UserName",
                        new {table = _table, user.UserName});
            var enumerable = sqlResult as dynamic[] ?? sqlResult.ToArray();
            if (!enumerable.Any())
                return null;
            dynamic firstOrDefault = enumerable.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Id : null;
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (user.Id == null)
                throw new ArgumentNullException(nameof(user));
            var sqlResult =
                await _connection.QueryAsync("SELECT UserName FROM @table WHERE Id=@Id", new {Key = user.Id});
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
                        _connection.QueryAsync("SELECT Name FROM @table WHERE UserName=@UserName",
                            new {table = _table, user.UserName});
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
                    await _connection.QueryAsync("SELECT Name FROM @table WHERE Id=@Id", new {Key = user.Id});
                var arr = sqlResult as dynamic[] ?? sqlResult.ToArray();
                if (!arr.Any())
                    return null;
                dynamic firstOrDefault = arr.FirstOrDefault();
                if (firstOrDefault != null) return firstOrDefault.Name;
                return null;
            }
        }

        public async Task<TUser> FindUserByIdAsync(string id, CancellationToken token)
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
                    _connection.QueryAsync("SELECT Id,Name,UserName FROM @table WHERE UserName=@Name",
                        new {table = _table, Name = name})).FirstOrDefault();
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
                    _connection.QueryAsync("SELECT Id,Name,UserName FROM @table WHERE Name=@name",
                        new {table = _table, name}));
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
            try
            {
                var succeeded = (await
                    _connection.ExecuteAsync("UPDATE @table SET Id=@id WHERE UserName=@username",
                        new {table = _table, username = user.UserName})) == 1;
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

        public async Task<QueryResult> SetUserNameAsync(TUser user, string username, CancellationToken token)
        {
            Header(token);
            if (user?.Id == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            try
            {
                var succeeded =
                    (await
                        _connection.ExecuteAsync("UPDATE @table SET UserName=@User WHERE Id=@Id",
                            new {table = _table, Key = user.Id, User = username})) == 1;
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
                            _connection.ExecuteAsync("UPDATE @table SET Name=@name WHERE Id=@Id", new {table = _table})) ==
                        1;
                else
                    succeeded =
                        (await
                            _connection.ExecuteAsync("UPDATE @table SET Name=@name WHERE UserName=@username",
                                new {table = _table, username = user.UserName, name})) ==
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
                var succeeded = (await _connection.ExecuteAsync("UPDATE @table SET Active=@isActive WHERE Id=@Id", new
                {
                    user.Id,
                    table = _table,
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

        public async Task<QueryResult> AddUserAsync(TUser user, string guid, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException(nameof(guid));
            try
            {
                var succeeded =
                    (await
                        (_connection.ExecuteAsync(
                            "INSERT INTO @table (Id,UserName,Name,Active) VALUES(@Id, @UserName, @Name, false)",
                            new {table = _table, Id = guid, user.UserName, user.Name}))) == 1;
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

        public async Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var succeeded =
                    (await _connection.ExecuteAsync("DELETE FROM @table WHERE Id=@Id", new {user.Id})) == 1;
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
                        _connection.ExecuteAsync("UPDATE @table SET UserName=@UserName, Name=@Name WHERE Id=@Id",
                            new {table = _table, user.UserName, user.Name, user.Id}) == 1;
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
            return (await _connection.QueryAsync("SELECT * FROM @table WHERE Id=@Id", new {Key = user.Id})).Count() == 1;
        }

        public async Task<bool> HasUserNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.UserName))
                throw new ArgumentNullException(nameof(user));
            return
                (await _connection.QueryAsync("SELECT * FROM @table WHERE UserName=@UserName", new {user.UserName}))
                    .Count() == 1;
        }

        public async Task<bool> HasNameAsync(TUser user, CancellationToken token)
        {
            Header(token);
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Id))
                throw new ArgumentNullException(nameof(user));
            return
                (await
                    _connection.QueryAsync("SELECT * FROM @table WHERE Name=@Name AND Id=@Id",
                        new {user.Name, Key = user.Id})).Count() == 1;
        }

        public async Task<bool> HasActivated(TUser user, CancellationToken token)
        {
            Header(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return (await _connection.QueryAsync("SELECT * FROM @table WHERE Id=@Id AND Active=true")).Any();
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