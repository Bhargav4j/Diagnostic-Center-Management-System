using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

public interface ITestEntryService
{
    Task<IEnumerable<TestEntryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestEntryDto?> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<TestEntryDto> CreateAsync(TestEntryCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestEntryUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestEntryDto>> GetUnpaidEntriesAsync(CancellationToken cancellationToken = default);
}
