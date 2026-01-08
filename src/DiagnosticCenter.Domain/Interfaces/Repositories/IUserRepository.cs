using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for User entity operations.
    /// Provides methods for CRUD operations and search functionality.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves all users from the repository.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of all users.</returns>
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="entity">The user entity to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The added user entity.</returns>
        Task<User> AddAsync(User entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="entity">The user entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(User entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a user from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a user exists in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for users matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against user properties.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of users matching the search criteria.</returns>
        Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
