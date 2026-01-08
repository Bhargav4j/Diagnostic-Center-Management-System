namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Payment operations
/// </summary>
public interface IPaymentService
{
    Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<object?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<object?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);

    Task<object> CreateAsync(object dto, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, object dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<object>> GetUnpaidBillsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<object>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
