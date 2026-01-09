using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface ITestTypeService
{
    Task<IEnumerable<TestType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestType> CreateAsync(TestType testType, CancellationToken cancellationToken = default);
    Task UpdateAsync(TestType testType, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestType>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
