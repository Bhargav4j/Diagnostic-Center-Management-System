using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for TestEntry entity operations.
    /// Provides methods for CRUD operations and search functionality.
    /// </summary>
    public interface ITestEntryRepository
    {
        /// <summary>
        /// Retrieves all test entries from the repository.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of all test entries.</returns>
        Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a test entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the test entry.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The test entry if found; otherwise, null.</returns>
        Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new test entry to the repository.
        /// </summary>
        /// <param name="entity">The test entry entity to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The added test entry entity.</returns>
        Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing test entry in the repository.
        /// </summary>
        /// <param name="entity">The test entry entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a test entry from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test entry to delete.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a test entry exists in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test entry.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the test entry exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for test entries matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against test entry properties.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of test entries matching the search criteria.</returns>
        Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
