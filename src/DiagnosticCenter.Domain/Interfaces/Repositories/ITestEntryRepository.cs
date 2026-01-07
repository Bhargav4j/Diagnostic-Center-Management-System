using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

public interface ITestEntryRepository
{
    Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default);
    Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetUnpaidTestsAsync(CancellationToken cancellationToken = default);
}
