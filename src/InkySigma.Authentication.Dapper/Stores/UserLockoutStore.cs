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
    public class UserLockoutStore : IUserLockoutStore<User>
    {
        private readonly SqlConnection _connection;
        private readonly string _table;

        public UserLockoutStore(SqlConnection conn, string format = "", string table = "auth.lockout")
        {
            _connection = conn;
            _table = table;
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

        public async Task<DateTime> GetLockoutEndDateTime(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            dynamic firstOrDefault =
                (await
                    _connection.QueryAsync("SELECT AccessEndDate FROM @table WHERE Id=@Id",
                        new {table = _table, Key = user.Id})).FirstOrDefault();
            if (firstOrDefault == null)
                throw new InvalidUserException(user.Id);
            return DateTime.Parse(firstOrDefault.Date);
        }

        public async Task<int> GetAccessFailedCount(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            dynamic firstOrDefault =
                (await
                    _connection.QueryAsync("SELECT AccessFailedCount FROM @table WHERE Id=@Id",
                        new {table = _table, Key = user.Id})).FirstOrDefault();
            if (firstOrDefault == null)
                throw new InvalidUserException(user.Id);
            return firstOrDefault.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabled(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            var value = await _connection.QueryAsync<bool>("SELECT LockoutEnabled FROM @table WHERE Id=@Id",
                new {Key = user.Id, table = _table});
            return value.Any();
        }

        public async Task<QueryResult> AddUserLockout(User user, CancellationToken token)
        {
            Handle(token);
            if (string.IsNullOrEmpty(user?.Id))
                throw new ArgumentNullException(nameof(user));
            try
            {
                await
                    _connection.ExecuteAsync(
                        "INSERT INTO @table (Id, AccessFailedCount, LockoutEnabled, AccessEndDate) VALUES(@Id, 0, false, @Date)",
                        new
                        {
                            table = _table,
                            user.Id
                        });
                return QueryResult.Success();
            }
            catch (SqlException e)
            {
                return BuildError(e);
            }
        }

        public Task<QueryResult> RemoveUserLockout(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetLockoutEndDateTime(User user, DateTime dateTime, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> SetLockoutEnabled(User user, bool isLockedOut, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrememntAccessFailedCount(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> ResetAccessFailedCount(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UserLockoutStore));
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