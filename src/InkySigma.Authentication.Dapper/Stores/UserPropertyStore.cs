using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;
using Npgsql;

namespace InkySigma.Authentication.Dapper.Stores
{
    /// <summary>
    /// No properties to CRUD on so far outside of authentication
    /// </summary>
    public class UserPropertyStore : IUserPropertyStore<User>
    {
        public NpgsqlConnection Connection { get; }
        public string Table { get; }

        public bool IsDisposed { get; private set; }

        public UserPropertyStore(NpgsqlConnection connection, string table = "auth.properties")
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

        public async Task<User> GetProperties(User user, CancellationToken token)
        {
            return user;
        }

        public async Task<QueryResult> RemoveProperties(User user, CancellationToken token)
        {
            return QueryResult.Success();
        }

        public async Task<QueryResult> UpdateProperties(User user, CancellationToken token)
        {
            return QueryResult.Success();
        }

        public async Task<QueryResult> AddProperties(User user, CancellationToken token)
        {
            return QueryResult.Success();
        }
    }
}