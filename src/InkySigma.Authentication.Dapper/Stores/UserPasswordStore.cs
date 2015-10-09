using System;
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
    public class UserPasswordStore : IUserPasswordStore<User>
    {
        private readonly SqlConnection _connection;
        private readonly string _table;
        public bool IsDisposed;

        public UserPasswordStore(SqlConnection connection, string table = "auth.pass")
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

        public async Task<string> GetPasswordAsync(User user, CancellationToken token = default(CancellationToken))
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

        public async Task<byte[]> GetSaltAsync(User user, CancellationToken token = default(CancellationToken))
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

        public async Task<QueryResult> AddPasswordAsync(User user, string password, byte[] salt, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var rowCount =
                    await
                        _connection.ExecuteAsync("INSERT INTO @table(Id,Password,Salt) VALUES(@Id,@Password,@Salt)",
                            new {table = _table, user.Id, Password = password, Salt = salt});
                return new QueryResult
                {
                    Succeeded = rowCount == 1
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> RemovePasswordAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var rowCount =
                    await _connection.ExecuteAsync("DELETE FROM @table WHERE Id=@Id", new {table = _table, user.Id});
                return new QueryResult
                {
                    Succeeded = rowCount == 1
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> SetPasswordAsync(User user, string password, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var rowCount =
                    await
                        _connection.ExecuteAsync("UPDATE @table SET Password=@password WHERE Id=@Id",
                            new {table = _table, password, Key = user.Id});
                return new QueryResult
                {
                    Succeeded = rowCount == 1
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<QueryResult> SetSaltAsync(User user, byte[] salt, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                var rowCount =
                    await
                        _connection.ExecuteAsync("UPDATE @table SET Salt=@salty WHERE Id=@Id",
                            new {table = _table, salty = Convert.ToBase64String(salt), Key = user.Id});
                return new QueryResult
                {
                    Succeeded = rowCount == 1
                };
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            return await _connection.ExecuteAsync("SELECT * FROM @table WHERE Id=@Id", new {Key = user.Id}) > 1;
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserPasswordStore));
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