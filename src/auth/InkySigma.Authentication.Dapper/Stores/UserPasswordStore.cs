﻿using System;
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
    public class UserPasswordStore<TUser> : IUserPasswordStore<TUser> where TUser : User
    {
        private readonly DbConnection _connection;
        private readonly string _table;
        public bool IsDisposed;

        public UserPasswordStore(DbConnection connection, string table = "auth.pass")
        {
            _connection = connection;
            _table = table;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _connection.Dispose();
                IsDisposed = true;
            }
        }

        public async Task<string> GetPasswordAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var arr =
                await
                    _connection.QueryAsync("SELECT Password FROM @table WHERE Id=@Id",
                        new {table = _table, Key = user.Id});
            var result = arr.FirstOrDefault();
            if (result == null)
                throw new InvalidUserException(user.Id);
            return result.Password;
        }

        public async Task<byte[]> GetSaltAsync(TUser user, CancellationToken token = default(CancellationToken))
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var arr =
                await
                    _connection.QueryAsync("SELECT Salt FROM @table WHERE Id=@Id", new {table = _table, Key = user.Id});
            var result = arr.FirstOrDefault();
            if (result == null)
                throw new InvalidUserException(user.Id);
            return Convert.FromBase64String(result.Salt);
        }

        public async Task<QueryResult> AddPasswordAsync(TUser user, string password, byte[] salt, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var rowCount =
                await
                    _connection.ExecuteAsync("INSERT INTO @table (Id,Password,Salt) VALUES(@Id,@Password,@Salt)",
                        new {table = _table, user.Id, Password = password, Salt = salt});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> RemovePasswordAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var rowCount =
                await _connection.ExecuteAsync("DELETE FROM @table WHERE Id=@Id", new {table = _table, user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> SetPasswordAsync(TUser user, string password, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));

            var rowCount =
                await
                    _connection.ExecuteAsync("UPDATE @table SET Password=@password WHERE Id=@Id",
                        new {table = _table, password, Key = user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> SetSaltAsync(TUser user, byte[] salt, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
                var rowCount =
                    await
                        _connection.ExecuteAsync("UPDATE @table SET Salt=@salty WHERE Id=@Id",
                            new {table = _table, salty = Convert.ToBase64String(salt), Key = user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            return await _connection.ExecuteAsync("SELECT * FROM @table WHERE Id=@Id", new {Key = user.Id}) > 1;
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserPasswordStore<TUser>));
            token.ThrowIfCancellationRequested();
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