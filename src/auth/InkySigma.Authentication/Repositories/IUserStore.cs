using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;

namespace InkySigma.Authentication.Repositories
{
    /// <summary>
    /// This store representing the user ID, the username, and the name.
    /// </summary>
    /// <typeparam name="TUser">The user type.</typeparam>
    public interface IUserStore<TUser> : IDisposable where TUser : class
    {
        /// <summary>
        /// Gets the user Id. Note: Some other form of identification must be supplied. This id will be used to look up other properties.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A string represting the id.</returns>
        Task<string> GetUserIdAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A string representing the username.</returns>
        Task<string> GetUserNameAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A string representing the name.</returns>
        Task<string> GetNameAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Find the user by ID. Note: the returned user only includes Id, UserName, and Name.
        /// </summary>
        /// <param name="id">The ID to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A user with an ID, a username, and a name.</returns>
        Task<TUser> FindUserByIdAsync(string id, CancellationToken token);

        /// <summary>
        /// Find the user by username. Note: the returned user only includes Id, UserName, and Name. 
        /// </summary>
        /// <param name="name">The username to look up</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A user with an ID, a username, and a name.</returns>
        Task<TUser> FindUserByUserNameAsync(string name, CancellationToken token);

        /// <summary>
        /// Find the users by name. Note: the returned user only includes Id, UserName, and Name. In addition, there may be multiple users that fit the criteria.
        /// </summary>
        /// <param name="name">The name to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>An iterable collection of users with Id, username, and name.</returns>
        Task<IEnumerable<TUser>> FindUsersByNameAsync(string name, CancellationToken token);

        /// <summary>
        /// Updates the Id of a user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="userid">The Id to associate.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetUserIdAsync(TUser user, string userid, CancellationToken token);

        /// <summary>
        /// Updates the username of a user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="username">The username to associate</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetUserNameAsync(TUser user, string username, CancellationToken token);

        /// <summary>
        /// Updates the name of a user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="name">The name to update.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> SetNameAsync(TUser user, string name, CancellationToken token);

        /// <summary>
        /// Adds a user. For use with account creation.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="guid">The guid to associate</param>
        /// <param name="name">The name to associate</param>
        /// <param name="username">The username to associate.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> AddUserAsync(TUser user, string guid, string name, string username, CancellationToken token);

        /// <summary>
        /// Removes a user. For use with account deletion.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> RemoveUserAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Updates a user and all its components.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A QueryResult representing whether the action succeeded and how many rows were affected.</returns>
        Task<QueryResult> UpdateUserAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets whether a user has an Id set.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean representing whether a user has an Id set.</returns>
        Task<bool> HasUserIdAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets whether a user has a UserName set.
        /// </summary>
        /// <param name="user">The user to look up</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean representing whether the user has a username set.</returns>
        Task<bool> HasUserNameAsync(TUser user, CancellationToken token);

        /// <summary>
        /// Gets whether a user has a Name set.
        /// </summary>
        /// <param name="user">The user to look up.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A boolean representing whether the user has a name set.</returns>
        Task<bool> HasNameAsync(TUser user, CancellationToken token);
    }
}