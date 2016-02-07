using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Exceptions;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Dapper.Stores
{
    public class UserLoginStore<TUser> : IUserLoginStore<TUser> where TUser : User
    {
        public DbConnection Connection { get; }
        public bool IsDisposed { get; set; }

        private string Table { get; }

        public UserLoginStore(DbConnection connection, string table = "auth.login")
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
                throw new ObjectDisposedException(nameof(UserLoginStore<TUser>));
            token.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Gets all user logins asychronously
        /// </summary>
        /// <param name="user">The user to be queried for</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>A list of all tokens</returns>
        public async Task<IEnumerable<LoginToken>> GetUserLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(nameof(user));
            var result = await Connection.QueryAsync("SELECT * FROM @Table WHERE Id=@Id", new {user.Id, Table});
            if (result == null)
                throw new NullReferenceException(nameof(result));
            var objects = result as dynamic[] ?? result.ToArray();
            var enumerable = new LoginToken[objects.Count()];
            for (int i = 0; i < objects.Count(); i++)
            {
                enumerable[i] = objects[i] as LoginToken;
            }
            return enumerable;
        }

        /// <summary>
        /// Verifies whether a user is associated the a given token.
        /// </summary>
        /// <param name="user">The user to be queried for</param>
        /// <param name="token">The token to be looked for</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A boolean representing whether the token is associated with the user</returns>
        public async Task<bool> HasUserLoginAsync(TUser user, string token, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            var tokens = await Connection.QueryAsync("SELECT * FROM @Table WHERE Id=@Id and Token=@token", new {Table, user.Id, token});
            if (tokens == null)
                throw new NullReferenceException(nameof(tokens));
            return tokens.Any();
        }

        public async Task<QueryResult> AddUserLogin(TUser user, string token, string location, DateTime expiration, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(location))
                throw new ArgumentNullException(nameof(location));
            if (expiration == null)
                throw new ArgumentNullException(nameof(expiration));
            var count = await
                Connection.ExecuteAsync(
                    "INSERT INTO @Table (Id, Token, Location, Expiration) VALUES(@Id, @token, @location, @expiration)",
                    new {user.Id, token, location, expiration, Table});
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> RemoveUserLogin(TUser user, string token, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            var count =
                await
                    Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id AND Token=@token",
                        new {Table, user.Id, token});
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> RemoveUser(TUser user, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var count =
                await
                    Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id",
                        new {Table, user.Id});
            return QueryResult.Success(count);
        }
    }
}
