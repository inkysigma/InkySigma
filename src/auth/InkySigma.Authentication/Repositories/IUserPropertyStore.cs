using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents any extraneous properties that vary on the User type.
    /// </summary>
    /// <typeparam name="TUser">The User type.</typeparam>
    public interface IUserPropertyStore<TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Gets the properties
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The user with all extraneous property fields filled in.</returns>
        Task<TUser> GetProperties(TUser user, CancellationToken token);

        /// <summary>
        /// Removes the properties. For use with account deletion.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken token);

        /// <summary>
        /// Updates all extraneous properties for the user.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> UpdateProperties(TUser user, CancellationToken token);

        /// <summary>
        /// Adds a properties entry for a user. For use with account creation.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddUser(TUser user, CancellationToken token);
    }
}