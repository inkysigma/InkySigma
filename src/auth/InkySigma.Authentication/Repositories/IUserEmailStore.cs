using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    public interface IUserEmailStore<TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Returns TUser identifying the user.
        /// </summary>
        /// <param name="email">The email to be looked up</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>TUser with identification only. Does not include the other properties.</returns>
        Task<TUser> FindUserByEmailAsync(string email, CancellationToken token);

        /// <summary>
        /// Obtains the user email.
        /// </summary>
        /// <param name="user">The associated user.</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>The email as a string.</returns>
        Task<string> GetUserEmailAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Obtains whether the email is claimed.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns>A boolean representing whether the email is claimed</returns>
        Task<bool> GetUserEmailConfirmedAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Attaches an email entry to a user
        /// </summary>
        /// <param name="user">The user to attach</param>
        /// <param name="email">The email to attach</param>
        /// <param name="token">A cancellation token</param>
        /// <returns></returns>
        Task<QueryResult> AddUserEmailAsync(TUser user, string email, CancellationToken token);

        /// <summary>
        /// Removes the user's entry completely. For use do delete a user entirely.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken token);

        /// <summary>
        /// Updates the email of the user
        /// </summary>
        /// <param name="user">The user to be updated</param>
        /// <param name="email">The new email</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetUserEmailAsync(TUser user, string email, CancellationToken token);

        /// <summary>
        /// Updates whether the user's email has been verified
        /// Note: This can be used to turn off a confirmation.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="isConfirmed">Whether the user has been confirmed</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetUserEmailConfirmedAsync(TUser user, bool isConfirmed, CancellationToken token);
        Task<bool> HasUserEmailAsync(TUser user, CancellationToken token);
    }
}