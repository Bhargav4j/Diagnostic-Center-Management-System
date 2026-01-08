using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for TestType entity operations.
    /// Provides methods for CRUD operations and search functionality.
    /// </summary>
    public interface ITestTypeRepository
    {
        /// <summary>
        /// Retrieves all test types from the repository.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of all test types.</returns>
        Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a test type by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the test type.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The test type if found; otherwise, null.</returns>
        Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new test type to the repository.
        /// </summary>
        /// <param name="entity">The test type entity to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The added test type entity.</returns>
        Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing test type in the repository.
        /// </summary>
        /// <param name="entity">The test type entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a test type from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test type to delete.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a test type exists in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the test type.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the test type exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for test types matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against test type properties.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of test types matching the search criteria.</returns>
        Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
