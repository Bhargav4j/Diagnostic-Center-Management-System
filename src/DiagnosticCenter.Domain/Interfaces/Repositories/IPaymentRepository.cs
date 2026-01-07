using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Payment entity
/// </summary>
public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
}
