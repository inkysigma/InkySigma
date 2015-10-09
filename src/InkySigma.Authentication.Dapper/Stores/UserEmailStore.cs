using System;
using System.Collections.Generic;
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
    public class UserEmailStore : IUserEmailStore<User>
    {
        private readonly string _table;
        private bool _isDisposed;
        private readonly SqlConnection _connection;

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

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UserEmailStore));
            token.ThrowIfCancellationRequested();
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
                throw new ArgumentNullException(user.UserName);
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            await _connection.QueryAsync("INSERT INTO @table Active FROM @table WHERE Id=@Id", new
            {
                table = _table,
                user.Id
            })

        }

        public Task<QueryResult> RemoveUserEmail(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetUserEmailAsync(User user, string email, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetUserEmailConfirmedAsync(User user, bool isConfirmed, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasUserEmailAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByEmailAsync(string email, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
