using System.Collections.Generic;
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
    public class UserPropertyStore<TUser> : IUserPropertyStore<TUser> where TUser : User
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

        public async Task<TUser> GetProperties(TUser user, CancellationToken token)
        {
            return user;
        }

        public async Task<QueryResult> RemoveProperties(TUser user, CancellationToken token)
        {
            return QueryResult.Success();
        }

        public async Task<QueryResult> UpdateProperties(TUser user, CancellationToken token)
        {
            return QueryResult.Success();
        }

        public async Task<QueryResult> AddProperties(TUser user, CancellationToken token)
        {
            var properties = user.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof (IDictionary<,>))
                {
                    
                }
            }
            return QueryResult.Success();
        }
    }
}