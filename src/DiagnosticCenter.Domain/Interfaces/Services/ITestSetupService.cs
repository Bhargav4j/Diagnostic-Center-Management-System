namespace DiagnosticCenter.Domain.Interfaces.Services;

/// <summary>
/// Service interface for TestSetup operations
/// </summary>
public interface ITestSetupService
{
    Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<object?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<object> CreateAsync(object dto, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, object dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<object>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
