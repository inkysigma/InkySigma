﻿using System;
using System.Data.Common;
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
    /// <summary>
    /// This is a store that stores user emails.
    /// </summary>
    public class UserEmailStore<TUser> : IUserEmailStore<TUser> where TUser : User
    {
        private readonly DbConnection _connection;
        public string Table { get; }
        private bool _isDisposed;

        public UserEmailStore(DbConnection connection, string table = "auth.email")
        {
            Table = table;
            _connection = connection;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _connection.Dispose();
            _isDisposed = true;
        }

        public async Task<string> GetUserEmailAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var first = (await _connection.QueryAsync($"SELECT Email FROM {Table} WHERE Id=@Id", new
            {
                user.Id
            })).FirstOrDefault();
            if (first == null)
                throw new InvalidUserException(user.UserName);
            return first;
        }

        public async Task<bool> GetUserEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var first = (await _connection.QueryAsync($"SELECT Active FROM {Table} WHERE Id=@Id", new
            {
                user.Id
            })).FirstOrDefault();
            if (first == null)
                throw new InvalidUserException(user.UserName);
            return first;
        }

        public async Task<QueryResult> AddUserEmailAsync(TUser user, string email, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            await _connection.ExecuteAsync($"INSERT INTO {Table} (Id, Email, Active) VALUES(@Id, @Email, false)", new
            {
                user.Id,
                email
            });
            return QueryResult.Success();
        }

        public async Task<QueryResult> RemoveUser(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            await _connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id", new {user.Id});
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetUserEmailAsync(TUser user, string email, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(email);
            await _connection.ExecuteAsync($"UPDATE {Table} SET Email=@email WHERE Id=@Id", new {email, user.Id });
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetUserEmailConfirmedAsync(TUser user, bool isConfirmed, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            await _connection.ExecuteAsync($"UPDATE {Table} SET Active=@isConfirmed WHERE Id=@Id", new { isConfirmed, user.Id});
            return QueryResult.Success();
        }

        public async Task<bool> HasUserEmailAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            var result = await _connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@Id", new {user.Id});
            if (result == null)
                throw new InvalidUserException();
            return result.Any();
        }

        public async Task<TUser> FindUserByEmailAsync(string email, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            var result =
                (await _connection.QueryAsync<string>($"SELECT Id FROM {Table} WHERE Email=@email", new {email}))
                    .FirstOrDefault();
            return (TUser) User.Create(result);
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserEmailStore<TUser>));
            token.ThrowIfCancellationRequested();
        }
    }
}