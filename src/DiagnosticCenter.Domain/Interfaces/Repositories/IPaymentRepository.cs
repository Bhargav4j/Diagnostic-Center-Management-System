using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Payment entity operations.
    /// Provides methods for CRUD operations and search functionality.
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// Retrieves all payments from the repository.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of all payments.</returns>
        Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a payment by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the payment.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The payment if found; otherwise, null.</returns>
        Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new payment to the repository.
        /// </summary>
        /// <param name="entity">The payment entity to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The added payment entity.</returns>
        Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing payment in the repository.
        /// </summary>
        /// <param name="entity">The payment entity with updated values.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a payment from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the payment to delete.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a payment exists in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the payment.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>True if the payment exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches for payments matching the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against payment properties.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A collection of payments matching the search criteria.</returns>
        Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}
