using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents the role of each user.
    /// </summary>
    /// <typeparam name="TUser">The User type.</typeparam>
    public interface IUserRoleStore<in TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Gets the user role.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cacnellation token.</param>
        /// <returns>A string array represting all the roles the user has.</returns>
        Task<string[]> GetUserRolesAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Adds a user. For use in account creation.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="role">The role to associate</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddUser(TUser user, string role, CancellationToken token);

        /// <summary>
        /// Removes a user role.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="role">The associated role to remove.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected. If the role does not exist for the user or</returns>
        Task<QueryResult> RemoveUserRoleAsync(TUser user, string role, CancellationToken token);

        /// <summary>
        /// Removes a user. For use with account deletion.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUser(TUser user, CancellationToken token);

        /// <summary>
        /// Checks whether the user is associated with a given role.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="role">The role to associate</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean represeting whether the user is associated with a role.</returns>
        Task<bool> HasRoleRoleAsync(TUser user, string role, CancellationToken token);
    }
}