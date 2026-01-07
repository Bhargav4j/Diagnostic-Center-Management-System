using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestType operations
/// </summary>
public interface ITestTypeService
{
    Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestType> CreateAsync(TestType entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestType entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
