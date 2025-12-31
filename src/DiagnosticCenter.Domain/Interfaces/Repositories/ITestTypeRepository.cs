using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TestType entity operations
/// </summary>
public interface ITestTypeRepository
{
    Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<TestType> AddAsync(TestType entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestType entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
