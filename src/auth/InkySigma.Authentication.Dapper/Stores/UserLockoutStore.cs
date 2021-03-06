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
    public class UserLockoutStore<TUser> : IUserLockoutStore<TUser> where TUser : User
    {
        private readonly DbConnection _connection;
        private string Table { get; }

        public UserLockoutStore(DbConnection conn, string table = "auth.lockout")
        {
            _connection = conn;
            Table = table;
        }

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                _connection.Dispose();
            }
        }

        public async Task<DateTime> GetLockoutEndDateTime(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            dynamic firstOrDefault =
                (await
                    _connection.QueryAsync($"SELECT AccessEndDate FROM {Table} WHERE Id=@Id",
                        new {Key = user.Id})).FirstOrDefault();
            if (firstOrDefault == null)
                throw new InvalidUserException(user.Id);
            return DateTime.Parse(firstOrDefault.Date);
        }

        public async Task<int> GetAccessFailedCount(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            dynamic firstOrDefault =
                (await
                    _connection.QueryAsync($"SELECT AccessFailedCount FROM {Table} WHERE Id=@Id",
                        new {Key = user.Id})).FirstOrDefault();
            if (firstOrDefault == null)
                throw new InvalidUserException(user.Id);
            return firstOrDefault.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabled(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var value = await _connection.QueryAsync<bool>($"SELECT LockoutEnabled FROM {Table} WHERE Id=@Id",
                new {Key = user.Id});
            return value.Any();
        }

        public async Task<QueryResult> AddUserLockout(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            await
                _connection.ExecuteAsync(
                    $"INSERT INTO {Table} (Id, AccessFailedCount, LockoutEnabled, AccessEndDate) VALUES(@Id, 0, false, @Date)",
                    new
                    {
                        user.Id
                    });
            return QueryResult.Success();
        }

        public async Task<QueryResult> RemoveUser(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            await
                _connection.ExecuteAsync(
                    $"DELETE FROM {Table} WHERE Id=@Id",
                    new
                    {
                        user.Id
                    });
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetLockoutEndDateTime(TUser user, DateTime dateTime, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            await
                _connection.ExecuteAsync($"UPDATE {Table} SET AccessEndDate=@dateTime WHERE Id=@Id",
                    new {dateTime, user.Id});
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetLockoutEnabled(TUser user, bool isLockedOut, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            await
                _connection.ExecuteAsync($"UPDATE {Table} SET LockoutEnabled=@isLockedOut WHERE Id=@Id",
                    new {isLockedOut, user.Id});
            return QueryResult.Success();
        }

        public async Task<int> IncrememntAccessFailedCount(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            int accessFailedCount = await GetAccessFailedCount(user, token);
            accessFailedCount++;
            await
                _connection.ExecuteAsync($"UPDATE {Table} SET AccessFailedCount=@accessFailedCount WHERE Id=@Id", new { accessFailedCount, user.Id});
            return accessFailedCount;
        }

        public async Task<QueryResult> ResetAccessFailedCount(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            await
                _connection.ExecuteAsync($"UPDATE {Table} SET AccessFailedCount=0 WHERE Id=@Id", new { user.Id });
            return QueryResult.Success();
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserLockoutStore<TUser>));
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