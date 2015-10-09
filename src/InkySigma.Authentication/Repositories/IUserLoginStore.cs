﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Repositories.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserLoginStore<in TUser> : IDisposable where TUser : class
    {
        Task<IEnumerable<TokenRow>> GetUserLoginsAsync(TUser user, CancellationToken token);
        Task<bool> HasUserLoginAsync(TUser user, string token, CancellationToken cancellationToken);

        Task<QueryResult> AddUserLogin(TUser user, string userToken, string location, DateTime expiration,
            CancellationToken token);

        Task<QueryResult> RemoveUserLogin(TUser user, string userToken, CancellationToken token);
        Task<QueryResult> RemoveUser(TUser user, CancellationToken token);
    }
}