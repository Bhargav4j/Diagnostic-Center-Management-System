using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

/// <summary>
/// Defines the contract for payment service operations.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Retrieves all payments asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of payment DTOs.</returns>
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific payment by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the payment.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment DTO if found; otherwise, null.</returns>
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new payment asynchronously.
    /// </summary>
    /// <param name="createDto">The data transfer object containing the information needed to create a payment.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created payment DTO.</returns>
    Task<PaymentDto> CreateAsync(PaymentCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the payment to update.</param>
    /// <param name="updateDto">The data transfer object containing the updated information.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(int id, PaymentUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the payment to delete.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for payments by bill number asynchronously.
    /// </summary>
    /// <param name="searchTerm">The search term to match against bill numbers.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of matching payment DTOs.</returns>
    Task<IEnumerable<PaymentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
