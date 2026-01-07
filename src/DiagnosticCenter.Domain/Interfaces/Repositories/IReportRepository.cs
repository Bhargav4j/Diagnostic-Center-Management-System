using DiagnosticCenter.Domain.Entities;

namespace DiagnosticCenter.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Report entity
/// </summary>
public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Report?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Report> AddAsync(Report report, CancellationToken cancellationToken = default);
    Task UpdateAsync(Report report, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Report>> GetByTestEntryIdAsync(int testEntryId, CancellationToken cancellationToken = default);
}
