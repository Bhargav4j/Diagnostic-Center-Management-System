using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for TestSetup entity operations.
    /// Provides methods for CRUD operations and search functionality.
    /// </summary>
    public interface ITestSetupRepository
    {
        /// <summary>
        /// Retrieves all test setups from the repository.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of all test setups.</returns>
        Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a test setup by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the test setup.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The test setup if found; otherwise, null.</returns>
        Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new test setup to the repository.
        /// </summary>
        /// <param name="entity">The test setup entity to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The added test setup entity.</returns>
        Task<TestSetup> AddAsync(TestSetup entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing test setup in the repository.
        /// </summary>
        /// <param name="entity">The test setup entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(TestSetup entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a test setup from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test setup to delete.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a test setup exists in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test setup.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the test setup exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for test setups matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against test setup properties.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of test setups matching the search criteria.</returns>
        Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
