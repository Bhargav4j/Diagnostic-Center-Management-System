using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Payment> CreateAsync(Payment entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, Payment entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
