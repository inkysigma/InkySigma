using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents the different logins and the associated keys.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    public interface IUserLoginStore<in TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Gets the logins the user has.
        /// </summary>
        /// <param name="user">The user to look up</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A iterable set of Logins.</returns>
        Task<IEnumerable<LoginToken>> GetUserLoginsAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Verifies whether the user has the token
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">The token to look up.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A boolean represting whether the user has the token.</returns>
        Task<bool> HasUserLoginAsync(TUser user, string token, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new login token for a user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="token">The token to be associated.</param>
        /// <param name="location">The location to be associated.</param>
        /// <param name="expiration">The expiration date of the token</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddUserLogin(TUser user, string token, string location, DateTime expiration,
            CancellationToken cancellationToken);

        /// <summary>
        /// Remoes a login associated with a user.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">The token to remove</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUserLogin(TUser user, string token, CancellationToken cancellationToken);

        /// <summary>
        /// Removes all logins associated with a user. For use with account deletion.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken cancellationToken);
    }
}