using System;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store represents the hashed password set and the salt required.
    /// Note: the hashed passwords are stored as strings while the salt is stored as a byte array.
    /// </summary>
    /// <typeparam name="TUser">The User type.</typeparam>
    public interface IUserPasswordStore<in TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The hashed password as a string.</returns>
        Task<string> GetPasswordAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The associated salt as a byte array.</returns>
        Task<byte[]> GetSaltAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Adds a password to a user. For use with account creation.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="password">The password to be associated.</param>
        /// <param name="salt">The salt to be associated.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddPasswordAsync(TUser user, string password, byte[] salt, CancellationToken token);

        /// <summary>
        /// Removes a password from a user. For use with account deletion.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemovePasswordAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Updates a password for a user. Use in conjunction with SetSalt if a new salt was generated.
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="password">The password to be associated.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetPasswordAsync(TUser user, string password, CancellationToken token);

        /// <summary>
        /// Updates a salt for a user. Use in conjunction with SetPassword if a new password was generated.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="salt">The salt to be associated.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetSaltAsync(TUser user, byte[] salt, CancellationToken token);

        /// <summary>
        /// Checks whether a user has an associated password.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean represeting whether the user has an associated password.</returns>
        Task<bool> HasPasswordAsync(TUser user, CancellationToken token);
    }
}