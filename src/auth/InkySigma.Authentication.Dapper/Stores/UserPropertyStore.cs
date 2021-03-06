﻿using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;

namespace InkySigma.Authentication.Dapper.Stores
{
    /// <summary>
    /// No properties to CRUD on so far outside of authentication
    /// </summary>
    public class UserPropertyStore<TUser> : IUserPropertyStore<TUser> where TUser : User
    {
        public DbConnection Connection { get; }
        public string Table { get; }

        public bool IsDisposed { get; private set; }

        public UserPropertyStore(DbConnection connection, string table = "auth.properties")
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

        public Task<TUser> GetProperties(TUser user, CancellationToken token)
        {
            return Task.Run(() => user, token);
        }

        public Task<QueryResult> RemoveUser(TUser user, CancellationToken token)
        {
            return Task.Run(() => QueryResult.Success(), token);
        }

        public Task<QueryResult> UpdateProperties(TUser user, CancellationToken token)
        {
            return Task.Run(() => QueryResult.Success(), token);
        }

        public Task<QueryResult> AddUser(TUser user, CancellationToken token)
        {
            /**var properties = user.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof (IDictionary<,>))
                {
                    
                }
                property.GetValue(user);
            }**/
            return Task.Run(() => QueryResult.Success(), token);
        }
    }
}