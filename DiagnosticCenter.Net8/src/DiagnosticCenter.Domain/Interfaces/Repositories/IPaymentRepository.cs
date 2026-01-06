using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Payment entity
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Gets all payments
    /// </summary>
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a payment by identifier
    /// </summary>
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payments by bill number
    /// </summary>
    Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payments by test entry
    /// </summary>
    Task<IEnumerable<Payment>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new payment
    /// </summary>
    Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment
    /// </summary>
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a payment exists
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches payments
    /// </summary>
    Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
