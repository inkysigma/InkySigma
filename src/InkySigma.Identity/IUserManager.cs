using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InkySigma.Identity.Repositories.Result;

namespace InkySigma.Identity
{
    public interface IUserManager<TUser> : IDisposable where TUser : class
    {
        Task<QueryResult> AddUserAsync(TUser user);
        Task<string> AddUserLoginAsync(TUser user, string password);

        Task<TUser> GetUserById(string userId);
        Task<IEnumerable<string>> GetUserRolesAsync(TUser user);
        Task<string> GetUserId(TUser user);
        Task<string> GetUserEmailAsync(TUser user);

        /// <summary>
        /// Update a user's email
        /// </summary>
        /// <param name="user">The user to be updated</param>
        /// <param name="email">The email</param>
        /// <returns></returns>
        Task<QueryResult> UpdateEmail(TUser user, string email);
        /// <summary>
        /// Update a user's password
        /// </summary>
        /// <param name="user">The user to be updated</param>
        /// <param name="password">A user's plain text password</param>
        /// <returns></returns>
        Task<QueryResult> UpdatePassword(TUser user, string password);

        Task<QueryResult> RemvoeUser(TUser user);
        Task<QueryResult> RemoveUserById(string userId);
    }
}
