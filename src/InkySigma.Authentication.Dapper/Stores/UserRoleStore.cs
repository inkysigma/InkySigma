using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;
using Npgsql;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserRoleStore<TUser> : IUserRoleStore<TUser> where TUser : User
    {
        public NpgsqlConnection Connection { get; }
        public string Table { get; }

        public bool IsDisposed { get; private set; }


        public UserRoleStore(NpgsqlConnection connection, string table = "auth.roles")
        {
            Connection = connection;
            Table = table;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            Connection.Dispose();
            IsDisposed = true;
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserRoleStore<TUser>));
            token.ThrowIfCancellationRequested();
        }

        public async Task<string[]> GetUserRolesAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var result =
                await Connection.QueryAsync<string>("SELECT Role FROM @Table WHERE Id=@Id", new {Table, user.Id});
            return result.ToArray();
        }

        public async Task<QueryResult> AddUserRoleAsync(TUser user, string role, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            var rowCount =
                await
                    Connection.ExecuteAsync("INSERT INTO @Table (Id,Role) VALUES(@Id, @role)",
                        new {Table, user.Id, role});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> RemoveUserRoleAsync(TUser user, string role, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            var rowCount =
                await
                    Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id AND Role=@role", new {Table, user.Id, role});
            return QueryResult.Success(rowCount);
        }

        public async Task<QueryResult> RemoveUserRolesAsync(TUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var rowCount =
                await
                    Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id", new {Table, user.Id});
            return QueryResult.Success(rowCount);
        }

        public async Task<bool> HasRoleRoleAsync(TUser user, string role, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            var result =
                await
                    Connection.QueryAsync("SELECT * FROM @Table WHERE Id=@Id AND Role=@role", new {user.Id, Table, role});
            return result.Any();
        }
    }
}
