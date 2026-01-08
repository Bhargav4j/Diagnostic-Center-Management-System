using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestEntry entity
/// </summary>
public interface ITestEntryRepository
{
    Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntry> AddAsync(TestEntry testEntry, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestEntry testEntry, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntry?> GetByBillNumberAsync(string billNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
