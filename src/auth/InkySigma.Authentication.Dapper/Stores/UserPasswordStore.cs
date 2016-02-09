using System;
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
        public string Table { get; }
        public bool IsDisposed;

        public UserPasswordStore(DbConnection connection, string table = "auth.pass")
        {
            _connection = connection;
            Table = table;
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
                    _connection.QueryAsync($"SELECT Password FROM {Table} WHERE Id=@Id",
                        new {Key = user.Id});
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
                    _connection.QueryAsync($"SELECT Salt FROM {Table} WHERE Id=@Id", new { Key = user.Id});
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
                    _connection.ExecuteAsync($"INSERT INTO {Table} (Id,Password,Salt) VALUES(@Id,@Password,@Salt)",
                        new { user.Id, Password = password, Salt = salt});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> RemovePasswordAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var rowCount =
                await _connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id", new {user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> SetPasswordAsync(TUser user, string password, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));

            var rowCount =
                await
                    _connection.ExecuteAsync($"UPDATE {Table} SET Password=@password WHERE Id=@Id",
                        new {password, Key = user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> SetSaltAsync(TUser user, byte[] salt, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
                var rowCount =
                    await
                        _connection.ExecuteAsync($"UPDATE {Table} SET Salt=@salty WHERE Id=@Id",
                            new { salty = Convert.ToBase64String(salt), Key = user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            return await _connection.ExecuteAsync($"SELECT * FROM {Table} WHERE Id=@Id", new {Key = user.Id}) > 1;
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserPasswordStore<TUser>));
            token.ThrowIfCancellationRequested();
        }
    }
}