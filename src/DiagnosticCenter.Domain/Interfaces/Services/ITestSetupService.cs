using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Services;

public interface ITestSetupService
{
    Task<IEnumerable<TestSetup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetup?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetup> CreateAsync(TestSetup entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestSetup entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetup>> GetByTestTypeIdAsync(int testTypeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetup>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
