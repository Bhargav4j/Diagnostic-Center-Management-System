using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestSetup entity operations
/// </summary>
public interface ITestSetupRepository
{
    Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetup> AddAsync(TestSetup testSetup, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestSetup testSetup, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetup>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);
}
