using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents whether a user should be locked out for too many login attempts.
    /// Note: The field exists even if the user is not locked out.
    /// </summary>
    /// <typeparam name="TUser">The User type</typeparam>
    public interface IUserLockoutStore<in TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Get the when the lockout ends. 
        /// Note: if there is no lockout, this returns null.
        /// </summary>
        /// <param name="user">The user to look up</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>Returns a DateTime representing the time lockout ends.</returns>
        Task<DateTime> GetLockoutEndDateTime(TUser user, CancellationToken token);

        /// <summary>
        /// Get the number of times the login attempts have failed
        /// </summary>
        /// <param name="user">The user to look up</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>The number of unsuccessful attempts.</returns>
        Task<int> GetAccessFailedCount(TUser user, CancellationToken token);

        /// <summary>
        /// Get whether the user is locked out.
        /// </summary>
        /// <param name="user">The user to look up</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean representing whether the user is locked out.</returns>
        Task<bool> GetLockoutEnabled(TUser user, CancellationToken token);

        /// <summary>
        /// Add a user's entry. To be used in conjunction with creating a user.
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddUserLockout(TUser user, CancellationToken token);

        /// <summary>
        /// Removes a user. To be used when deleting a user across all databases
        /// </summary>
        /// <param name="user">The user to remove</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken token);

        /// <summary>
        /// Update the lockout end date.
        /// </summary>
        /// <param name="user">The target user.</param>
        /// <param name="dateTime">The end date time.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetLockoutEndDateTime(TUser user, DateTime dateTime, CancellationToken token);

        /// <summary>
        /// Update whether the user is locked out.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="isLockedOut">Whether the user is locked out.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns></returns>
        Task<QueryResult> SetLockoutEnabled(TUser user, bool isLockedOut, CancellationToken token);

        /// <summary>
        /// Incrememnts the number of failed accesses.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The number of failed access attempts.</returns>
        Task<int> IncrememntAccessFailedCount(TUser user, CancellationToken token);

        /// <summary>
        /// Resets the number of failed accesses.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> ResetAccessFailedCount(TUser user, CancellationToken token);
    }
}