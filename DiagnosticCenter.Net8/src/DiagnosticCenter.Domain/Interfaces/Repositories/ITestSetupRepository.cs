using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestSetup entity
/// </summary>
public interface ITestSetupRepository
{
    /// <summary>
    /// Gets all test setups
    /// </summary>
    Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a test setup by identifier
    /// </summary>
    Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets test setups by type
    /// </summary>
    Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new test setup
    /// </summary>
    Task<TestSetup> AddAsync(TestSetup testSetup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test setup
    /// </summary>
    Task UpdateAsync(TestSetup testSetup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test setup
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a test setup exists
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches test setups by name
    /// </summary>
    Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
