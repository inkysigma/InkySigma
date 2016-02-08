using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents tokens for resetting and account modifications. Note that if the user has no tokens, there are no entries.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    public interface IUserTokenStore<in TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Adds a token for a user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="token">The token to associate.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddTokenAsync(TUser user, UpdateToken token, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the tokens for a user.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<IEnumerable<UpdateToken>> GetTokensAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Find a token based on user and token string.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="code">The token to look up.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>An update token that holds the code, expiration, and type.</returns>
        Task<UpdateToken> FindTokenAsync(TUser user, string code, CancellationToken cancellationToken);

        /// <summary>
        /// Removes a given token.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="code">The code to remove.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveTokenAsync(TUser user, string code, CancellationToken cancellationToken);

        /// <summary>
        /// Removes all tokens associated with a user.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken cancellationToken);
    }
}