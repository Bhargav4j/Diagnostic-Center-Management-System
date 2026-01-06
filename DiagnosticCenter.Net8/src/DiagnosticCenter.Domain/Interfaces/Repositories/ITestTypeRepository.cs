using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestType entity
/// </summary>
public interface ITestTypeRepository
{
    /// <summary>
    /// Gets all test types
    /// </summary>
    Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a test type by identifier
    /// </summary>
    Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new test type
    /// </summary>
    Task<TestType> AddAsync(TestType testType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing test type
    /// </summary>
    Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a test type
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a test type exists
    /// </summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a test type name already exists
    /// </summary>
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches test types by name
    /// </summary>
    Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
