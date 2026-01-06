using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestEntry entity
/// </summary>
public interface ITestEntryRepository
{
    /// <summary>
    /// Gets all test entries
    /// </summary>
    Task<IEnumerable<TestEntry>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a test entry by identifier
    /// </summary>
    Task<TestEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a test entry by bill number
    /// </summary>
    Task<TestEntry?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets unpaid test entries
    /// </summary>
    Task<IEnumerable<TestEntry>> GetUnpaidAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new test entry
    /// </summary>
    Task<TestEntry> AddAsync(TestEntry testEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test entry
    /// </summary>
    Task UpdateAsync(TestEntry testEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test entry
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a test entry exists
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches test entries by patient name or bill number
    /// </summary>
    Task<IEnumerable<TestEntry>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
