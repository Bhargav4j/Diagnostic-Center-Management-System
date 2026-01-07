using DiagnosticCenter.Application.DTOs;

namespace DiagnosticCenter.Application.Interfaces;

public interface ITestSetupService
{
    Task<IEnumerable<TestSetupDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TestSetupDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TestSetupDto> CreateAsync(TestSetupCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TestSetupUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TestSetupDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
