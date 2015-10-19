using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class UserUpdateTokenStore : IUserUpdateTokenStore<User>
    {
        public SqlConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;
        public string Table { get; }

        public UserUpdateTokenStore(SqlConnection connection, string table = "auth.token")
        {
            Connection = connection;
            Table = table;
        }

        private void Handle(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserUpdateTokenStore));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<QueryResult> AddTokenAsync(User user, UpdateTokenRow token, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            if (token.Validate())
                throw new ArgumentOutOfRangeException(nameof(token));
            var count = await
                Connection.ExecuteAsync(
                    "INSERT INTO @Table (Id, Token, Expiration, Property) VALUES(@Id, @Token, @Expiration, @Property)",
                    new
                    {
                        user.Id,
                        token.Token,
                        token.Expiration,
                        Property = Convert.ToInt32(token.Property),
                        Table
                    });
            return QueryResult.Success(count);
        }

        public async Task<IEnumerable<UpdateTokenRow>> GetTokensAsync(User user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var results = await Connection.QueryAsync("SELECT * FROM @Table WHERE Id=@Id", new
            {
                user.Id,
                Table
            });
            var enumerable = results as dynamic[] ?? results.ToArray();
            var list = new UpdateTokenRow[enumerable.Count()];
            for (int i = 0; i < enumerable.Count(); i++)
            {
                list[i] = new UpdateTokenRow
                {
                    Expiration = enumerable[i].Expiration,
                    Token = enumerable[i].Token,
                    Property = (UpdateProperty)enumerable[i].Property
                };
            }
            return list;
        }

        public async Task<UpdateTokenRow> FindTokenAsync(User user, string code, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            var result =
                await Connection.QueryAsync<UpdateTokenRow>("SELECT * FROM @Table WHERE Id=@Id And Token=@code", new {Table, user.Id, code});
            return result.FirstOrDefault();
        }

        public async Task<QueryResult> RemoveTokenAsync(User user, string code, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            var result =
                await Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id AND Token=@code", new { Table, user.Id, code });
            return QueryResult.Success(result);
        }

        public async Task<QueryResult> RemoveUserAsync(User user, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidUserException(user.UserName);
            var result =
                await Connection.ExecuteAsync("DELETE FROM @Table WHERE Id=@Id", new { Table, user.Id});
            return QueryResult.Success(result);
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserUpdateTokenStore));
            IsDisposed = true;
        }
    }
}
