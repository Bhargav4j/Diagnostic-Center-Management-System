using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestEntry business operations
/// </summary>
public interface ITestEntryService
{
    Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntry> CreateAsync(TestEntry entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestEntry entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default);
}
