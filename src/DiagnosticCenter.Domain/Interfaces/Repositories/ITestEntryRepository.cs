using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestEntry entity operations
/// </summary>
public interface ITestEntryRepository
{
    Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetByMobileNoAsync(string mobileNo, CancellationToken cancellationToken = default);
    Task<TestEntry> AddAsync(TestEntry entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestEntry entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default);
}
