﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Repositories;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Dapper.Stores
{
    /// <summary>
    /// This is a store that stores user emails.
    /// </summary>
    public class UserEmailStore : IUserEmailStore<User>
    {
        private readonly SqlConnection _connection;
        private readonly string _table;
        private bool _isDisposed;

        public UserEmailStore(SqlConnection connection, string table = "auth.email")
        {
            _table = table;
            _connection = connection;
        }

        public void Dispose()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserEmailStore));
            _connection.Dispose();
            _isDisposed = true;
        }

        public async Task<string> GetUserEmailAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var first = (await _connection.QueryAsync("SELECT Email FROM @table WHERE Id=@Id", new
            {
                table = _table,
                user.Id
            })).FirstOrDefault();
            if (first == null)
                throw new InvalidUserException(user.UserName);
            return first;
        }

        public async Task<bool> GetUserEmailConfirmedAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var first = (await _connection.QueryAsync("SELECT Active FROM @table WHERE Id=@Id", new
            {
                table = _table,
                user.Id
            })).FirstOrDefault();
            if (first == null)
                throw new InvalidUserException(user.UserName);
            return first;
        }

        public async Task<QueryResult> AddUserEmailAsync(User user, string email, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            await _connection.ExecuteAsync("INSERT INTO @table(Id, Email, Active) VALUES(@Id, @Email, false)", new
            {
                table = _table,
                user.Id,
                email
            });
            return QueryResult.Success();
        }

        public async Task<QueryResult> RemoveUserEmail(User user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            await _connection.ExecuteAsync("DELETE FROM @table WHERE Id=@Id", new {table = _table, user.Id});
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetUserEmailAsync(User user, string email, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(email);
            await _connection.ExecuteAsync("UPDATE @table SET Email=@email WHERE Id=@Id", new {email, user.Id, table = _table});
            return QueryResult.Success();
        }

        public async Task<QueryResult> SetUserEmailConfirmedAsync(User user, bool isConfirmed, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            await _connection.ExecuteAsync("UPDATE @table SET Active=@isConfirmed WHERE Id=@Id", new { isConfirmed, user.Id, table = _table });
            return QueryResult.Success();
        }

        public async Task<bool> HasUserEmailAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.UserName))
                throw new InvalidUserException(user.UserName);
            var result = await _connection.QueryAsync("SELECT * FROM @table WHERE Id=@Id", new {table = _table, user.Id});
            if (result == null)
                throw new InvalidUserException();
            return result.Any();
        }

        public Task<User> FindUserByEmailAsync(string email, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserEmailStore));
            token.ThrowIfCancellationRequested();
        }
    }
}